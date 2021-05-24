package services

import (
	"context"
	"go.mongodb.org/mongo-driver/bson"
	"go.mongodb.org/mongo-driver/bson/primitive"
	"go.mongodb.org/mongo-driver/mongo/options"
	"sgc-server/packages/config"
	"sgc-server/packages/models"
	"time"
)

// GetContacts busca contactos en la base de datos.
func GetContacts(page int64, search string) ([]*models.Contact, bool) {
	ctx, cancel := context.WithTimeout(context.Background(), 15*time.Second)
	defer cancel()

	data := config.MongoConnect.Database(config.Database)
	col := data.Collection(config.ContactCollection)

	var results []*models.Contact

	findOptions := options.Find()
	findOptions.SetSkip((page - 1) * 20)
	findOptions.SetLimit(20)

	query := bson.M{
		"full_name":  bson.M{"$regex": `(?i)` + search},
		"is_deleted": false,
	}

	cursor, err := col.Find(ctx, query, findOptions)
	if err != nil {
		return results, false
	}

	for cursor.Next(ctx) {
		var doc models.Contact
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

// GetContact busca un contacto en la BD.
func GetContact(ID string) (models.Contact, error) {
	ctx, cancel := context.WithTimeout(context.Background(), 15*time.Second)
	defer cancel()

	data := config.MongoConnect.Database(config.Database)
	col := data.Collection(config.ContactCollection)

	var contact models.Contact
	objID, _ := primitive.ObjectIDFromHex(ID)

	filter := bson.M{
		"_id": objID,
	}

	err := col.FindOne(ctx, filter).Decode(&contact)

	if err != nil {
		return contact, err
	}
	return contact, nil
}

// AddContact registra los datos de los contactos.
func AddContact(doc models.Contact) (string, bool, error) {
	ctx, cancel := context.WithTimeout(context.Background(), 15*time.Second)
	defer cancel()

	data := config.MongoConnect.Database(config.Database)
	col := data.Collection(config.ContactCollection)

	doc.IsDeleted = false

	result, err := col.InsertOne(ctx, doc)
	if err != nil {
		return "", false, err
	}
	objID, _ := result.InsertedID.(primitive.ObjectID)
	return objID.Hex(), true, nil
}

// UpdateContact actualiza el registro de contactos..
func UpdateContact(doc models.Contact, ID string) (bool, error) {
	ctx, cancel := context.WithTimeout(context.Background(), 15*time.Second)
	defer cancel()

	data := config.MongoConnect.Database(config.Database)
	col := data.Collection(config.ContactCollection)

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

// DeleteContact hace un borrado lógico del documento.
func DeleteContact(ID string) (bool, error) {
	ctx, cancel := context.WithTimeout(context.Background(), 15*time.Second)
	defer cancel()

	data := config.MongoConnect.Database(config.Database)
	col := data.Collection(config.ContactCollection)

	arrData := make(map[string]interface{})
	arrData["is_deleted"] = true

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
