package repository

import (
	"context"
	"go.mongodb.org/mongo-driver/bson"
	"go.mongodb.org/mongo-driver/bson/primitive"
	"go.mongodb.org/mongo-driver/mongo/options"
	"sgc-server/packages/config"
	"sgc-server/packages/models"
	"time"
)

// GetTaxes buscar impuestos.
func GetTaxes(page int64, search string) ([]*models.Tax, bool) {
	ctx, cancel := context.WithTimeout(context.Background(), 15*time.Second)
	defer cancel()

	data := config.MongoConnect.Database(config.Database)
	col := data.Collection(config.TaxCollection)

	var results []*models.Tax

	findOptions := options.Find()
	findOptions.SetSkip((page - 1) * 20)
	findOptions.SetLimit(20)

	query := bson.M{
		"name":       bson.M{"$regex": `(?i)` + search},
		"is_deleted": false,
	}

	cursor, err := col.Find(ctx, query, findOptions)
	if err != nil {
		return results, false
	}

	for cursor.Next(ctx) {
		var doc models.Tax
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

// GetTax retorna un impuesto.
func GetTax(ID string) (models.Tax, error) {
	ctx, cancel := context.WithTimeout(context.Background(), 15*time.Second)
	defer cancel()

	data := config.MongoConnect.Database(config.Database)
	col := data.Collection(config.TaxCollection)

	var doc models.Tax
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

// AddTax agrega nuevo impuesto.
func AddTax(doc models.Tax) (string, bool, error) {
	ctx, cancel := context.WithTimeout(context.Background(), 15*time.Second)
	defer cancel()

	data := config.MongoConnect.Database(config.Database)
	col := data.Collection(config.TaxCollection)

	doc.IsDeleted = false

	result, err := col.InsertOne(ctx, doc)
	if err != nil {
		return "", false, err
	}
	objID, _ := result.InsertedID.(primitive.ObjectID)
	return objID.Hex(), true, nil
}

// UpdateTax actualiza los datos del impuesto
func UpdateTax(doc models.Tax, ID string) (bool, error) {
	ctx, cancel := context.WithTimeout(context.Background(), 15*time.Second)
	defer cancel()

	data := config.MongoConnect.Database(config.Database)
	col := data.Collection(config.TaxCollection)

	arrData := make(map[string]interface{})
	if len(doc.Name) > 0 {
		arrData["name"] = doc.Name
	}
	if doc.Value >= 0 {
		arrData["value"] = doc.Value
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

// DeleteTax borra un impuesto.
func DeleteTax(ID string) (bool, error) {
	ctx, cancel := context.WithTimeout(context.Background(), 15*time.Second)
	defer cancel()

	data := config.MongoConnect.Database(config.Database)
	col := data.Collection(config.TaxCollection)

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
