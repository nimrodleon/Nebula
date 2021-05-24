package repository

import (
	"context"
	"go.mongodb.org/mongo-driver/bson"
	"go.mongodb.org/mongo-driver/bson/primitive"
	"log"
	"sgc-server/packages/config"
	"sgc-server/packages/models"
	"time"
)

// GetOrderRepairs busca las ordenes de reparación.
func GetOrderRepairs(page int64, search string) ([]models.OrderRepairWithClient, bool) {
	ctx, cancel := context.WithTimeout(context.Background(), 15*time.Second)
	defer cancel()

	data := config.MongoConnect.Database(config.Database)
	col := data.Collection(config.ContactCollection)

	var results []models.OrderRepairWithClient

	//skip := (page - 1) * 20

	queries := make([]bson.M, 0)
	queries = append(queries, bson.M{
		"$match": bson.M{
			"full_name": bson.M{"$regex": `(?i)` + search},
		},
	})
	queries = append(queries, bson.M{
		"$lookup": bson.M{
			"from":         config.OrderRepairCollection,
			"localField":   "_id",
			"foreignField": "client_id",
			"as":           "OrderRepair",
		}})
	queries = append(queries, bson.M{
		"$project": bson.M{
			"full_name": true,
			"OrderRepair": bson.M{
				"$filter": bson.M{
					"input": "$OrderRepair",
					"as":    "rep",
					"cond": bson.M{
						//"$eq": bson.A{"$$rep.is_deleted", false},
						"$and": bson.A{
							bson.M{"$eq": bson.A{"$$rep.equipo_entregado", "N"}},
							bson.M{"$eq": bson.A{"$$rep.is_deleted", false}},
						},
					}},
			},
		},
	})
	queries = append(queries, bson.M{"$unwind": "$OrderRepair"})
	queries = append(queries, bson.M{"$sort": bson.M{"OrderRepair.created_at": -1}})
	//queries = append(queries, bson.M{"$skip": skip})
	//queries = append(queries, bson.M{"$limit": 20})

	cursor, err := col.Aggregate(ctx, queries)

	if err != nil {
		return results, false
	}

	// Cargar los resultados del cursor.
	if err = cursor.All(ctx, &results); err != nil {
		return results, false
	}
	log.Println(results)
	return results, true
}

// GetOrderRepair retorna una orden de reparación.
func GetOrderRepair(ID string) (models.OrderRepair, error) {
	ctx, cancel := context.WithTimeout(context.Background(), 15*time.Second)
	defer cancel()

	data := config.MongoConnect.Database(config.Database)
	col := data.Collection(config.OrderRepairCollection)

	var doc models.OrderRepair
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

// AddOrderRepair agrega nueva orden de reparación.
func AddOrderRepair(doc models.OrderRepair) (string, bool, error) {
	ctx, cancel := context.WithTimeout(context.Background(), 15*time.Second)
	defer cancel()

	data := config.MongoConnect.Database(config.Database)
	col := data.Collection(config.OrderRepairCollection)

	doc.Status = models.StatusOrderRepairSinRevisar
	doc.EquipoEntregado = "N"
	doc.IsDeleted = false
	doc.CreatedAt = time.Now()

	result, err := col.InsertOne(ctx, doc)
	if err != nil {
		return "", false, err
	}
	objID, _ := result.InsertedID.(primitive.ObjectID)
	return objID.Hex(), true, nil
}

// UpdateOrderRepair actualizar datos de la orden de reparación.
func UpdateOrderRepair(doc models.OrderRepair, ID string) (bool, error) {
	ctx, cancel := context.WithTimeout(context.Background(), 15*time.Second)
	defer cancel()

	data := config.MongoConnect.Database(config.Database)
	col := data.Collection(config.OrderRepairCollection)

	arrData := make(map[string]interface{})
	if len(doc.ReceptionDate) > 0 {
		arrData["reception_date"] = doc.ReceptionDate
	}
	if len(doc.ReceptionTime) > 0 {
		arrData["reception_time"] = doc.ReceptionTime
	}
	if len(doc.MemberUserId.Hex()) > 0 {
		arrData["member_user_id"] = doc.MemberUserId
	}
	if len(doc.ClientId.Hex()) > 0 {
		arrData["client_id"] = doc.ClientId
	}
	if len(doc.DeviceTypeId.Hex()) > 0 {
		arrData["device_type_id"] = doc.DeviceTypeId
	}
	if len(doc.WarehouseId.Hex()) > 0 {
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
	if len(doc.TechnicalUserId.Hex()) > 0 {
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

// DeleteOrderRepair borra una orden de reparación.
func DeleteOrderRepair(ID string) (bool, error) {
	ctx, cancel := context.WithTimeout(context.Background(), 15*time.Second)
	defer cancel()

	data := config.MongoConnect.Database(config.Database)
	col := data.Collection(config.OrderRepairCollection)

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
