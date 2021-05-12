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
	col := data.Collection(db.ContactCollection)

	var results []models.OrderRepairWithClient

	skip := (page - 1) * 20

	queries := make([]bson.M, 0)
	queries = append(queries, bson.M{
		"$lookup": bson.M{
			"from":         db.OrderRepairCollection,
			"localField":   "_id",
			"foreignField": "client_id",
			"as":           "OrderRepair",
		}})
	queries = append(queries, bson.M{
		"$project": bson.M{
			"full_name": true,
			"OrderRepair": bson.M{
				"$filter": bson.M{
					"input": "$OrderRepair",
					"as":    "rep",
					"cond": bson.M{
						"$eq": bson.A{"$$rep.is_deleted", false},
					}},
			},
		},
	})
	queries = append(queries, bson.M{"$unwind": "$OrderRepair"})
	queries = append(queries, bson.M{"$sort": bson.M{"OrderRepair.created_at": -1}})
	queries = append(queries, bson.M{"$skip": skip})
	queries = append(queries, bson.M{"$limit": 20})

	cursor, err := col.Aggregate(ctx, queries)

	if err != nil {
		return results, false
	}

	// Cargar los resultados del cursor.
	if err = cursor.All(ctx, &results); err != nil {
		return results, false
	}
	log.Println(results)
	return results, true
}
