package models

import (
	"github.com/dgrijalva/jwt-go"
	"go.mongodb.org/mongo-driver/bson/primitive"
)

type Claim struct {
	ID         primitive.ObjectID `bson:"_id" json:"_id,omitempty"`
	UserName   string             `json:"user_name"`
	Email      string             `json:"email"`
	Permission string             `json:"permission"`
	jwt.StandardClaims
}
