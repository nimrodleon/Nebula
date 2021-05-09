package user

import (
	"context"
	"go.mongodb.org/mongo-driver/bson"
	"go.mongodb.org/mongo-driver/bson/primitive"
	"sgt-server/database/db"
	"sgt-server/models"
	"time"
)

// GetUser retorno un usuario.
func GetUser(ID string) (models.User, error) {
	ctx, cancel := context.WithTimeout(context.Background(), 15*time.Second)
	defer cancel()

	data := db.MongoConnect.Database(db.Database)
	col := data.Collection(db.UserCollection)

	var doc models.User
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
