package warehouse

import (
	"context"
	"go.mongodb.org/mongo-driver/bson"
	"sgt-server/database/db"
	"sgt-server/models"
	"time"
)

// GetWarehousesWithType retorna la lista de almacenes filtrado por tipo.
func GetWarehousesWithType(typeWarehouse string, search string) ([]*models.Warehouse, bool) {
	ctx, cancel := context.WithTimeout(context.Background(), 15*time.Second)
	defer cancel()

	data := db.MongoConnect.Database(db.Database)
	col := data.Collection(db.WarehouseCollection)

	var results []*models.Warehouse

	query := bson.M{
		"type":       typeWarehouse,
		"name":       bson.M{"$regex": `(?i)` + search},
		"is_deleted": false,
	}

	cursor, err := col.Find(ctx, query)
	if err != nil {
		return results, false
	}

	for cursor.Next(ctx) {
		var doc models.Warehouse
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
