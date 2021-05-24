package controllers

import (
	"encoding/json"
	"fmt"
	"github.com/gorilla/mux"
	"net/http"
	"sgc-server/packages/jwt"
	"sgc-server/packages/middlew"
	"sgc-server/packages/models"
	"sgc-server/packages/repository"
	"strconv"
)

func ArticleRouterHandler(router *mux.Router) {
	router.HandleFunc("/api/articles", middlew.CheckDb(middlew.ValidateJWT(GetArticlesHandler))).Methods("GET")
	router.HandleFunc("/api/articles/{id}", middlew.CheckDb(middlew.ValidateJWT(GetArticleHandler))).Methods("GET")
	router.HandleFunc("/api/articles", middlew.CheckDb(middlew.ValidateJWT(AddArticleHandler))).Methods("POST")
	router.HandleFunc("/api/articles/{id}", middlew.CheckDb(middlew.ValidateJWT(EditArticleHandler))).Methods("PUT")
	router.HandleFunc("/api/articles/{id}", middlew.CheckDb(middlew.ValidateJWT(DeleteArticleHandler))).Methods("DELETE")
}

// GetArticlesHandler carga la lista de artículos.
func GetArticlesHandler(w http.ResponseWriter, r *http.Request) {
	page := r.URL.Query().Get("page")
	search := r.URL.Query().Get("search")

	pagTemp, err := strconv.Atoi(page)
	if err != nil {
		http.Error(w, "Debe enviar el parámetro página como entero mayor a 0", http.StatusBadRequest)
		return
	}

	pag := int64(pagTemp)

	result, status := repository.GetArticles(pag, search)
	if status == false {
		http.Error(w, "Error al leer los artículos", http.StatusBadRequest)
		return
	}

	w.Header().Set("Content-Type", "application/json")
	w.WriteHeader(http.StatusCreated)
	_ = json.NewEncoder(w).Encode(result)
}

// GetArticleHandler devuelve un artículo.
func GetArticleHandler(w http.ResponseWriter, r *http.Request) {
	vars := mux.Vars(r)
	articleId := vars["id"]
	if len(articleId) < 1 {
		http.Error(w, "Debe enviar el parámetro ID", http.StatusBadRequest)
		return
	}

	doc, err := repository.GetArticle(articleId)
	if err != nil {
		http.Error(w, "Ocurrió un error al intentar buscar el registro, "+err.Error(), http.StatusBadRequest)
		return
	}

	w.Header().Set("Content-Type", "application/json")
	w.WriteHeader(http.StatusCreated)
	_ = json.NewEncoder(w).Encode(doc)
}

// AddArticleHandler agrega un articulo.
func AddArticleHandler(w http.ResponseWriter, r *http.Request) {
	var doc models.Article
	err := json.NewDecoder(r.Body).Decode(&doc)
	if err != nil {
		http.Error(w, "Error en los datos recibidos "+err.Error(), http.StatusBadRequest)
		return
	}

	objID, status, err := repository.AddArticle(doc)
	if err != nil {
		http.Error(w, "Ocurrió un error al intentar realizar el registro "+err.Error(), http.StatusBadRequest)
		return
	}
	if status == false {
		http.Error(w, "No se ha logrado insertar el registro", http.StatusBadRequest)
		return
	}
	w.WriteHeader(http.StatusCreated)
	_, _ = fmt.Fprint(w, objID)
}

// EditArticleHandler actualiza un articulo en la DB.
func EditArticleHandler(w http.ResponseWriter, r *http.Request) {
	vars := mux.Vars(r)
	articleId := vars["id"]

	var doc models.Article

	err := json.NewDecoder(r.Body).Decode(&doc)
	if err != nil {
		http.Error(w, "Datos Incorrectos "+err.Error(), http.StatusBadRequest)
		return
	}

	status, err := repository.UpdateArticle(doc, articleId)
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

// DeleteArticleHandler borra un artículo.
func DeleteArticleHandler(w http.ResponseWriter, r *http.Request) {
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

	_, err := repository.DeleteArticle(articleId)
	if err != nil {
		http.Error(w, "Ocurrió un error al intentar borrar, "+err.Error(), http.StatusBadRequest)
		return
	}

	w.Header().Set("Content-Type", "application/json")
	w.WriteHeader(http.StatusCreated)
}
