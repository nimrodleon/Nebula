package articleHandler

import (
	"github.com/gorilla/mux"
	"net/http"
	"sgt-server/database/article"
	"sgt-server/routes/jwt"
)

// DeleteArticle borra un artículo.
func DeleteArticle(w http.ResponseWriter, r *http.Request) {
	vars := mux.Vars(r)
	articleId := vars["id"]
	if len(articleId) < 1 {
		http.Error(w, "Debe enviar el parámetro ID", http.StatusBadRequest)
		return
	}

	if !jwt.CheckDeletePermission() {
		http.Error(w, "Error no tiene permiso para eliminar este documento", http.StatusBadRequest)
		return
	}

	_, err := article.DeleteArticle(articleId)
	if err != nil {
		http.Error(w, "Ocurrió un error al intentar borrar, "+err.Error(), http.StatusBadRequest)
		return
	}

	w.Header().Set("Content-Type", "application/json")
	w.WriteHeader(http.StatusCreated)
}
