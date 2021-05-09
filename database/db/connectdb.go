package db

import (
	"context"
	"go.mongodb.org/mongo-driver/mongo"
	"go.mongodb.org/mongo-driver/mongo/options"
	"log"
)

// MongoConnect es el objeto de conexión a la DB.
var MongoConnect = ConnectDb()

// clientOptions tiene la URI de conexión.
var clientOptions = options.Client().ApplyURI("mongodb://127.0.0.1:27017/" + Database)

// ConnectDb Permite conectar a la base de datos.
func ConnectDb() *mongo.Client {
	client, err := mongo.Connect(context.TODO(), clientOptions)
	if err != nil {
		log.Fatal(err.Error())
		return client
	}
	err = client.Ping(context.TODO(), nil)
	if err != nil {
		log.Fatal(err.Error())
		return client
	}
	log.Println("mongodb connected successfully")
	return client
}

// CheckConnection es el ping a la DB.
func CheckConnection() int {
	err := MongoConnect.Ping(context.TODO(), nil)
	if err != nil {
		return 0
	}
	return 1
}
