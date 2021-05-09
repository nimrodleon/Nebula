package contact

import (
	"context"
	"go.mongodb.org/mongo-driver/bson"
	"go.mongodb.org/mongo-driver/bson/primitive"
	"sgc-server/database/db"
	"sgc-server/models"
	"time"
)

// UpdateContact actualiza el registro de contactos..
func UpdateContact(doc models.Contact, ID string) (bool, error) {
	ctx, cancel := context.WithTimeout(context.Background(), 15*time.Second)
	defer cancel()

	data := db.MongoConnect.Database(db.Database)
	col := data.Collection(db.ContactCollection)

	arrData := make(map[string]interface{})
	if len(doc.TypeDoc) > 0 {
		arrData["type_doc"] = doc.TypeDoc
	}
	if len(doc.Document) > 0 {
		arrData["document"] = doc.Document
	}
	if len(doc.FullName) > 0 {
		arrData["full_name"] = doc.FullName
	}
	if len(doc.Address) > 0 {
		arrData["address"] = doc.Address
	}
	if len(doc.PhoneNumber) > 0 {
		arrData["phone_number"] = doc.PhoneNumber
	}
	if len(doc.Email) > 0 {
		arrData["email"] = doc.Email
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
