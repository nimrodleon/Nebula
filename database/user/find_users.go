package user

import (
	"context"
	"go.mongodb.org/mongo-driver/bson"
	"go.mongodb.org/mongo-driver/mongo/options"
	"sgc-server/database/db"
	"sgc-server/models"
	"time"
)

// FindUsers buscar usuarios.
func FindUsers(page int64, search string) ([]*models.User, bool) {
	ctx, cancel := context.WithTimeout(context.Background(), 15*time.Second)
	defer cancel()

	data := db.MongoConnect.Database(db.Database)
	col := data.Collection(db.UserCollection)

	var results []*models.User

	findOptions := options.Find()
	findOptions.SetSkip((page - 1) * 20)
	findOptions.SetLimit(20)

	query := bson.M{
		"full_name":  bson.M{"$regex": `(?i)` + search},
		"permission": bson.M{"$in": bson.A{"ROLE_ADMIN", "ROLE_CASH", "ROLE_USER"}},
		"is_deleted": false,
	}

	cursor, err := col.Find(ctx, query, findOptions)
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
