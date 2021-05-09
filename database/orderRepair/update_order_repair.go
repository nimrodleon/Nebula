package orderRepair

import (
	"context"
	"go.mongodb.org/mongo-driver/bson"
	"go.mongodb.org/mongo-driver/bson/primitive"
	"sgc-server/database/db"
	"sgc-server/models"
	"time"
)

// UpdateOrderRepair actualizar datos de la orden de reparación.
func UpdateOrderRepair(doc models.OrderRepair, ID string) (bool, error) {
	ctx, cancel := context.WithTimeout(context.Background(), 15*time.Second)
	defer cancel()

	data := db.MongoConnect.Database(db.Database)
	col := data.Collection(db.OrderRepairCollection)

	arrData := make(map[string]interface{})
	if len(doc.ReceptionDate) > 0 {
		arrData["reception_date"] = doc.ReceptionDate
	}
	if len(doc.ReceptionTime) > 0 {
		arrData["reception_time"] = doc.ReceptionTime
	}
	if len(doc.MemberUserId) > 0 {
		arrData["member_user_id"] = doc.MemberUserId
	}
	if len(doc.ClientId) > 0 {
		arrData["client_id"] = doc.ClientId
	}
	if len(doc.DeviceTypeId) > 0 {
		arrData["device_type_id"] = doc.DeviceTypeId
	}
	if len(doc.WarehouseId) > 0 {
		arrData["warehouse_id"] = doc.WarehouseId
	}
	if len(doc.DeviceInfo) > 0 {
		arrData["device_info"] = doc.DeviceInfo
	}
	if len(doc.Failure) > 0 {
		arrData["failure"] = doc.Failure
	}
	if len(doc.PromisedDate) > 0 {
		arrData["promised_date"] = doc.PromisedDate
	}
	if len(doc.PromisedTime) > 0 {
		arrData["promised_time"] = doc.PromisedTime
	}
	if len(doc.TechnicalUserId) > 0 {
		arrData["technical_user_id"] = doc.TechnicalUserId
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
