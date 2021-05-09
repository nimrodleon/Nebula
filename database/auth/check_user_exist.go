package auth

import (
	"context"
	"go.mongodb.org/mongo-driver/bson"
	"sgt-server/database/db"
	"sgt-server/models"
	"time"
)

// CheckUserExist recibe userName de parámetro y
// chequea si ya está en la base de datos.
func CheckUserExist(userName string) (models.User, bool, string) {
	ctx, cancel := context.WithTimeout(context.Background(), 15*time.Second)
	defer cancel()

	data := db.MongoConnect.Database(db.Database)
	col := data.Collection(db.UserCollection)

	condition := bson.M{"user_name": userName}

	var result models.User

	err := col.FindOne(ctx, condition).Decode(&result)
	ID := result.ID.Hex()
	if err != nil {
		return result, false, ID
	}
	return result, true, ID
}
