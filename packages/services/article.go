package services

import (
	"context"
	"go.mongodb.org/mongo-driver/bson"
	"go.mongodb.org/mongo-driver/bson/primitive"
	"go.mongodb.org/mongo-driver/mongo/options"
	"sgc-server/packages/config"
	"sgc-server/packages/models"
	"time"
)

// GetArticles buscar artículos.
func GetArticles(page int64, search string) ([]*models.Article, bool) {
	ctx, cancel := context.WithTimeout(context.Background(), 15*time.Second)
	defer cancel()

	data := config.MongoConnect.Database(config.Database)
	col := data.Collection(config.ArticleCollection)

	var results []*models.Article

	findOptions := options.Find()
	findOptions.SetSkip((page - 1) * 20)
	findOptions.SetLimit(20)

	query := bson.M{
		"is_deleted": false,
		"$or": []interface{}{
			bson.M{"name": bson.M{"$regex": `(?i)` + search}},
			bson.M{"bar_code": bson.M{"$regex": `(?i)` + search}},
		},
		// "name": bson.M{"$regex": `(?i)` + search},
	}

	cursor, err := col.Find(ctx, query, findOptions)
	if err != nil {
		return results, false
	}

	for cursor.Next(ctx) {
		var doc models.Article
		err := cursor.Decode(&doc)
		if err != nil {
			return results, false
		}
		results = append(results, &doc)
	}

	err = cursor.Err()
	if err != nil {
		return results, false
	}
	_ = cursor.Close(ctx)
	return results, true
}

// GetArticle devuelve un artículo.
func GetArticle(ID string) (models.Article, error) {
	ctx, cancel := context.WithTimeout(context.Background(), 15*time.Second)
	defer cancel()

	data := config.MongoConnect.Database(config.Database)
	col := data.Collection(config.ArticleCollection)

	var doc models.Article
	objID, _ := primitive.ObjectIDFromHex(ID)

	filter := bson.M{
		"_id": objID,
	}

	err := col.FindOne(ctx, filter).Decode(&doc)

	if err != nil {
		return doc, err
	}
	return doc, nil
}

// AddArticle agrega un artículo.
func AddArticle(doc models.Article) (string, bool, error) {
	ctx, cancel := context.WithTimeout(context.Background(), 15*time.Second)
	defer cancel()

	data := config.MongoConnect.Database(config.Database)
	col := data.Collection(config.ArticleCollection)

	doc.IsDeleted = false

	result, err := col.InsertOne(ctx, doc)
	if err != nil {
		return "", false, err
	}
	objID, _ := result.InsertedID.(primitive.ObjectID)
	return objID.Hex(), true, nil
}

// UpdateArticle actualizar artículo.
func UpdateArticle(doc models.Article, ID string) (bool, error) {
	ctx, cancel := context.WithTimeout(context.Background(), 15*time.Second)
	defer cancel()

	data := config.MongoConnect.Database(config.Database)
	col := data.Collection(config.ArticleCollection)

	arrData := make(map[string]interface{})
	if len(doc.Name) > 0 {
		arrData["name"] = doc.Name
	}
	if len(doc.Type) > 0 {
		arrData["type"] = doc.Type
	}
	if len(doc.BarCode) > 0 {
		arrData["bar_code"] = doc.BarCode
	}
	if len(doc.TaxID) > 0 {
		arrData["tax_id"] = doc.TaxID
	}
	arrData["price_0"] = doc.Price0
	arrData["price_1"] = doc.Price1
	if len(doc.Remark) > 0 {
		arrData["remark"] = doc.Remark
	}

	updateString := bson.M{
		"$set": arrData,
	}

	objID, _ := primitive.ObjectIDFromHex(ID)
	filter := bson.M{"_id": bson.M{"$eq": objID}}

	_, err := col.UpdateOne(ctx, filter, updateString)
	if err != nil {
		return false, err
	}
	return true, nil
}

// DeleteArticle borrar artículo.
func DeleteArticle(ID string) (bool, error) {
	ctx, cancel := context.WithTimeout(context.Background(), 15*time.Second)
	defer cancel()

	data := config.MongoConnect.Database(config.Database)
	col := data.Collection(config.ArticleCollection)

	arrData := make(map[string]interface{})
	arrData["is_deleted"] = true

	updateString := bson.M{
		"$set": arrData,
	}

	objID, _ := primitive.ObjectIDFromHex(ID)
	filter := bson.M{"_id": bson.M{"$eq": objID}}

	_, err := col.UpdateOne(ctx, filter, updateString)
	if err != nil {
		return false, err
	}
	return true, nil
}
