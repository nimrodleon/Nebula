package orderRepair

import (
	"context"
	"go.mongodb.org/mongo-driver/bson"
	"log"
	"sgc-server/database/db"
	"sgc-server/models"
	"time"
)

// FindOrderRepairs busca las ordenes de reparación.
func FindOrderRepairs(page int64, search string) ([]models.OrderRepairWithClient, bool) {
	ctx, cancel := context.WithTimeout(context.Background(), 15*time.Second)
	defer cancel()

	data := db.MongoConnect.Database(db.Database)
	col := data.Collection(db.OrderRepairCollection)

	//var results []*models.OrderRepair
	var results []models.OrderRepairWithClient

	//findOptions := options.Find()
	//findOptions.SetSkip((page - 1) * 20)
	//findOptions.SetLimit(20)

	skip := (page - 1) * 20

	//query := bson.M{
	//	"failure":    bson.M{"$regex": `(?i)` + search},
	//	"is_deleted": false,
	//}
	query := make([]bson.M, 0)
	query = append(query, bson.M{
		"$lookup": bson.M{
			"from":         db.ContactCollection,
			"localField":   "client_id",
			"foreignField": "_id",
			"as":           "client",
		}})
	query = append(query, bson.M{"$unwind": "$client"})
	query = append(query, bson.M{"$skip": skip})
	query = append(query, bson.M{"$limit": 20})

	// cursor, err := col.Find(ctx, query, findOptions)
	cursor, err := col.Aggregate(ctx, query)
	//goland:noinspection GoNilness
	if err = cursor.All(ctx, &results); err != nil {
		return results, false
	}

	//err = cursor.All(ctx, &results)
	//if err != nil {
	//	return results, false
	//}

	log.Println("Test Results")
	log.Println(results)

	//for cursor.Next(ctx) {
	//	var doc models.OrderRepair
	//	err := cursor.Decode(&doc)
	//	if err != nil {
	//		return results, false
	//	}
	//	results = append(results, &doc)
	//}

	//err = cursor.Err()
	//if err != nil {
	//	return results, false
	//}
	//_ = cursor.Close(ctx)
	return results, true
}
