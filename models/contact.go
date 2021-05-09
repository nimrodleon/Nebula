package models

import "go.mongodb.org/mongo-driver/bson/primitive"

type Contact struct {
	ID          primitive.ObjectID `bson:"_id,omitempty" json:"id"`
	TypeDoc     string             `bson:"type_doc" json:"type_doc"`
	Document    string             `bson:"document" json:"document"`
	FullName    string             `bson:"full_name" json:"full_name"`
	Address     string             `bson:"address" json:"address"`
	PhoneNumber string             `bson:"phone_number" json:"phone_number"`
	Email       string             `bson:"email" json:"email"`
	IsDeleted   bool               `bson:"is_deleted" json:"is_deleted"`
}
