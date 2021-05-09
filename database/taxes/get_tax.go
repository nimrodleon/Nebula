package taxes

import (
	"context"
	"go.mongodb.org/mongo-driver/bson"
	"go.mongodb.org/mongo-driver/bson/primitive"
	"sgc-server/database/db"
	"sgc-server/models"
	"time"
)

// GetTax retorna un impuesto.
func GetTax(ID string) (models.Tax, error) {
	ctx, cancel := context.WithTimeout(context.Background(), 15*time.Second)
	defer cancel()

	data := db.MongoConnect.Database(db.Database)
	col := data.Collection(db.TaxCollection)

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
