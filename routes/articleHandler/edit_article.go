package articleHandler

import (
	"encoding/json"
	"github.com/gorilla/mux"
	"net/http"
	"sgt-server/database/article"
	"sgt-server/models"
)

// EditArticle actualiza un articulo en la DB.
func EditArticle(w http.ResponseWriter, r *http.Request) {
	vars := mux.Vars(r)
	articleId := vars["id"]

	var doc models.Article

	err := json.NewDecoder(r.Body).Decode(&doc)
	if err != nil {
		http.Error(w, "Datos Incorrectos "+err.Error(), http.StatusBadRequest)
		return
	}

	status, err := article.UpdateArticle(doc, articleId)
	if err != nil {
		http.Error(w, "Ocurrió un error al intentar modificar el registro. "+err.Error(), http.StatusBadRequest)
		return
	}
	if status == false {
		http.Error(w, "No se ha logrado modificar el registro", http.StatusBadRequest)
		return
	}

	w.WriteHeader(http.StatusCreated)
}
