package models

import (
	"go.mongodb.org/mongo-driver/bson/primitive"
	"time"
)

// StatusOrderRepairSinRevisar estado inicial.
const StatusOrderRepairSinRevisar string = "SIN REVISAR"

// OrderRepair modelo orden de reparación.
type OrderRepair struct {
	ID              primitive.ObjectID `bson:"_id,omitempty" json:"id"`
	ReceptionDate   string             `bson:"reception_date" json:"reception_date"`
	ReceptionTime   string             `bson:"reception_time" json:"reception_time"`
	MemberUserId    string             `bson:"member_user_id" json:"member_user_id"`
	ClientId        string             `bson:"client_id" json:"client_id"`
	DeviceTypeId    string             `bson:"device_type_id" json:"device_type_id"`
	WarehouseId     string             `bson:"warehouse_id" json:"warehouse_id"`
	DeviceInfo      string             `bson:"device_info" json:"device_info"`
	Failure         string             `bson:"failure" json:"failure"`
	PromisedDate    string             `bson:"promised_date" json:"promised_date"`
	PromisedTime    string             `bson:"promised_time" json:"promised_time"`
	TechnicalUserId string             `bson:"technical_user_id" json:"technical_user_id"`
	Status          string             `bson:"status" json:"status"`
	IsDeleted       bool               `bson:"is_deleted" json:"is_deleted"`
	CreatedAt       time.Time          `bson:"created_at" json:"created_at"`
}
