package models

import "go.mongodb.org/mongo-driver/bson/primitive"

type Article struct {
	ID        primitive.ObjectID `bson:"_id,omitempty" json:"id"`
	Name      string             `bson:"name" json:"name"`
	Type      string             `bson:"type" json:"type"`
	BarCode   string             `bson:"bar_code" json:"bar_code"`
	TaxID     string             `bson:"tax_id" json:"tax_id"`
	Price0    float64            `bson:"price_0" json:"price_0"`
	Price1    float64            `bson:"price_1" json:"price_1"`
	Remark    string             `bson:"remark" json:"remark"`
	IsDeleted bool               `bson:"is_deleted" json:"is_deleted"`
}
