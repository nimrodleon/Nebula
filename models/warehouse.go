package models

import "go.mongodb.org/mongo-driver/bson/primitive"

type Warehouse struct {
	ID        primitive.ObjectID `bson:"_id,omitempty" json:"id"`
	Type      string             `bson:"type" json:"type"`
	Name      string             `bson:"name" json:"name"`
	IsDeleted bool               `bson:"is_deleted" json:"is_deleted"`
}
