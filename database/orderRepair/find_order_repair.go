package orderRepair

import (
	"context"
	"go.mongodb.org/mongo-driver/bson"
	"go.mongodb.org/mongo-driver/mongo/options"
	"sgc-server/database/db"
	"sgc-server/models"
	"time"
)

// FindOrderRepairs busca las ordenes de reparación.
func FindOrderRepairs(page int64, search string) ([]*models.OrderRepair, bool) {
	ctx, cancel := context.WithTimeout(context.Background(), 15*time.Second)
	defer cancel()

	data := db.MongoConnect.Database(db.Database)
	col := data.Collection(db.OrderRepairCollection)

	var results []*models.OrderRepair

	findOptions := options.Find()
	findOptions.SetSkip((page - 1) * 20)
	findOptions.SetLimit(20)

	query := bson.M{
		"failure":    bson.M{"$regex": `(?i)` + search},
		"is_deleted": false,
	}

	cursor, err := col.Find(ctx, query, findOptions)
	if err != nil {
		return results, false
	}

	for cursor.Next(ctx) {
		var doc models.OrderRepair
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
