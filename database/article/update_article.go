package article

import (
	"context"
	"go.mongodb.org/mongo-driver/bson"
	"go.mongodb.org/mongo-driver/bson/primitive"
	"sgt-server/database/db"
	"sgt-server/models"
	"time"
)

// UpdateArticle actualizar artículo.
func UpdateArticle(doc models.Article, ID string) (bool, error) {
	ctx, cancel := context.WithTimeout(context.Background(), 15*time.Second)
	defer cancel()

	data := db.MongoConnect.Database(db.Database)
	col := data.Collection(db.ArticleCollection)

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
