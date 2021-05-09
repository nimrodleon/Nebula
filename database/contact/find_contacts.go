package contact

import (
	"context"
	"go.mongodb.org/mongo-driver/bson"
	"go.mongodb.org/mongo-driver/mongo/options"
	"sgt-server/database/db"
	"sgt-server/models"
	"time"
)

// FindContacts busca contactos en la base de datos.
func FindContacts(page int64, search string) ([]*models.Contact, bool) {
	ctx, cancel := context.WithTimeout(context.Background(), 15*time.Second)
	defer cancel()

	data := db.MongoConnect.Database(db.Database)
	col := data.Collection(db.ContactCollection)

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
