package deviceType

import (
	"context"
	"go.mongodb.org/mongo-driver/bson"
	"go.mongodb.org/mongo-driver/bson/primitive"
	"sgt-server/database/db"
	"sgt-server/models"
	"time"
)

// UpdateDeviceType actualiza el tipo de equipo.
func UpdateDeviceType(doc models.DeviceType, ID string) (bool, error) {
	ctx, cancel := context.WithTimeout(context.Background(), 15*time.Second)
	defer cancel()

	data := db.MongoConnect.Database(db.Database)
	col := data.Collection(db.DeviceTypeCollection)

	arrData := make(map[string]interface{})
	if len(doc.Name) > 0 {
		arrData["name"] = doc.Name
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
