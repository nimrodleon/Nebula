package main

import (
	"log"
	"sgc-server/database/db"
	"sgc-server/routes"
)

func main() {
	if db.CheckConnection() == 0 {
		log.Fatal("Sin conexión a la BD")
		return
	}
	routes.Handlers()
}
