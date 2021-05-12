package user

import (
	"context"
	"go.mongodb.org/mongo-driver/bson"
	"sgc-server/database/db"
	"sgc-server/models"
	"time"
)

// GetActiveUsers retorna la lista de usuarios activos.
func GetActiveUsers(search string) ([]*models.User, bool) {
	ctx, cancel := context.WithTimeout(context.Background(), 15*time.Second)
	defer cancel()

	data := db.MongoConnect.Database(db.Database)
	col := data.Collection(db.UserCollection)

	var results []*models.User

	query := bson.M{
		"suspended":  false,
		"full_name":  bson.M{"$regex": `(?i)` + search},
		"permission": bson.M{"$in": bson.A{"ROLE_ADMIN", "ROLE_CASH", "ROLE_USER"}},
		"is_deleted": false,
	}

	cursor, err := col.Find(ctx, query)
	if err != nil {
		return results, false
	}

	for cursor.Next(ctx) {
		var doc models.User
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
