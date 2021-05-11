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
	ReceptionDate   string             `bson:"reception_date" json:"reception_date,omitempty"`
	ReceptionTime   string             `bson:"reception_time" json:"reception_time"`
	MemberUserId    primitive.ObjectID `bson:"member_user_id" json:"member_user_id"`
	ClientId        primitive.ObjectID `bson:"client_id" json:"client_id"`
	DeviceTypeId    primitive.ObjectID `bson:"device_type_id" json:"device_type_id"`
	WarehouseId     primitive.ObjectID `bson:"warehouse_id" json:"warehouse_id"`
	DeviceInfo      string             `bson:"device_info" json:"device_info"`
	Failure         string             `bson:"failure" json:"failure"`
	PromisedDate    string             `bson:"promised_date" json:"promised_date"`
	PromisedTime    string             `bson:"promised_time" json:"promised_time"`
	TechnicalUserId primitive.ObjectID `bson:"technical_user_id" json:"technical_user_id"`
	Status          string             `bson:"status" json:"status"`
	IsDeleted       bool               `bson:"is_deleted" json:"is_deleted"`
	CreatedAt       time.Time          `bson:"created_at" json:"created_at"`
}

// OrderRepairWithClient ordenes de reparación con datos del cliente.
type OrderRepairWithClient struct {
	ID          string `bson:"_id" json:"id"`
	FullName    string `bson:"full_name" json:"full_name"`
	OrderRepair OrderRepair
}
