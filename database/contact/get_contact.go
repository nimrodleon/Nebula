package contact

import (
	"context"
	"go.mongodb.org/mongo-driver/bson"
	"go.mongodb.org/mongo-driver/bson/primitive"
	"sgt-server/database/db"
	"sgt-server/models"
	"time"
)

// GetContact busca un contacto en la BD.
func GetContact(ID string) (models.Contact, error) {
	ctx, cancel := context.WithTimeout(context.Background(), 15*time.Second)
	defer cancel()

	data := db.MongoConnect.Database(db.Database)
	col := data.Collection(db.ContactCollection)

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
