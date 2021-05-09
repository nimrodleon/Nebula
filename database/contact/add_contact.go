package contact

import (
	"context"
	"go.mongodb.org/mongo-driver/bson/primitive"
	"sgt-server/database/db"
	"sgt-server/models"
	"time"
)

// AddContact registra los datos de los contactos.
func AddContact(doc models.Contact) (string, bool, error) {
	ctx, cancel := context.WithTimeout(context.Background(), 15*time.Second)
	defer cancel()

	data := db.MongoConnect.Database(db.Database)
	col := data.Collection(db.ContactCollection)

	doc.IsDeleted = false

	result, err := col.InsertOne(ctx, doc)
	if err != nil {
		return "", false, err
	}
	objID, _ := result.InsertedID.(primitive.ObjectID)
	return objID.Hex(), true, nil
}
