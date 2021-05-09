package models

import "go.mongodb.org/mongo-driver/bson/primitive"

type User struct {
	ID          primitive.ObjectID `bson:"_id,omitempty" json:"id"`
	FullName    string             `bson:"full_name" json:"full_name,omitempty"`
	Address     string             `bson:"address" json:"address,omitempty"`
	PhoneNumber string             `bson:"phone_number" json:"phone_number,omitempty"`
	UserName    string             `bson:"user_name" json:"user_name,omitempty"`
	Password    string             `bson:"password" json:"password,omitempty"`
	Permission  string             `bson:"permission" json:"permission,omitempty"`
	Email       string             `bson:"email" json:"email,omitempty"`
	Avatar      string             `bson:"avatar" json:"avatar,omitempty"`
	Suspended   bool               `bson:"suspended" json:"suspended"`
	IsDeleted   bool               `bson:"is_deleted" json:"is_deleted"`
}
