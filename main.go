package main

import (
	"log"
	"sgt-server/database/db"
	"sgt-server/routes"
)

func main() {
	if db.CheckConnection() == 0 {
		log.Fatal("Sin conexión a la BD")
		return
	}
	routes.Handlers()
}
