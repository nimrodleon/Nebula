package user

import (
	"context"
	"go.mongodb.org/mongo-driver/bson"
	"go.mongodb.org/mongo-driver/bson/primitive"
	"sgt-server/database/db"
	"sgt-server/models"
	"time"
)

// UpdateUser actualizar usuario.
func UpdateUser(doc models.User, ID string) (bool, error) {
	ctx, cancel := context.WithTimeout(context.Background(), 15*time.Second)
	defer cancel()

	data := db.MongoConnect.Database(db.Database)
	col := data.Collection(db.UserCollection)

	arrData := make(map[string]interface{})
	if len(doc.FullName) > 0 {
		arrData["full_name"] = doc.FullName
	}
	if len(doc.Address) > 0 {
		arrData["address"] = doc.Address
	}
	if len(doc.PhoneNumber) > 0 {
		arrData["phone_number"] = doc.PhoneNumber
	}
	if len(doc.Permission) > 0 {
		arrData["permission"] = doc.Permission
	}
	if len(doc.Email) > 0 {
		arrData["email"] = doc.Email
	}
	if len(doc.Avatar) > 0 {
		arrData["avatar"] = doc.Avatar
	}

	updateString := bson.M{
		"$set": arrData,
	}

	objID, _ := primitive.ObjectIDFromHex(ID)
	filter := bson.M{"_id": bson.M{"$eq": objID}}

	_, err := col.UpdateOne(ctx, filter, updateString)
	if err != nil {
		return false, err
	}
	return true, nil
}
