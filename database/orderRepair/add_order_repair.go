package orderRepair

import (
	"context"
	"go.mongodb.org/mongo-driver/bson/primitive"
	"sgc-server/database/db"
	"sgc-server/models"
	"time"
)

// AddOrderRepair agrega nueva orden de reparación.
func AddOrderRepair(doc models.OrderRepair) (string, bool, error) {
	ctx, cancel := context.WithTimeout(context.Background(), 15*time.Second)
	defer cancel()

	data := db.MongoConnect.Database(db.Database)
	col := data.Collection(db.OrderRepairCollection)

	doc.IsDeleted = false
	doc.CreatedAt = time.Now()

	result, err := col.InsertOne(ctx, doc)
	if err != nil {
		return "", false, err
	}
	objID, _ := result.InsertedID.(primitive.ObjectID)
	return objID.Hex(), true, nil
}
