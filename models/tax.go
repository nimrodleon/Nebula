package models

import "go.mongodb.org/mongo-driver/bson/primitive"

type Tax struct {
	ID        primitive.ObjectID `bson:"_id,omitempty" json:"id"`
	Name      string             `bson:"name" json:"name"`
	Value     int32              `bson:"value" json:"value"`
	IsDeleted bool               `bson:"is_deleted" json:"is_deleted"`
}
