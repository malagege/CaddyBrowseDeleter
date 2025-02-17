package main

import (
	"log"
	"net/http"
	"os"
	"path/filepath"

	"github.com/gorilla/mux"
	"gorm.io/driver/sqlite"
	"gorm.io/gorm"
)

var db *gorm.DB

func main() {
	var err error
	db, err = gorm.Open(sqlite.Open("db/app.db"), &gorm.Config{})
	if err != nil {
		log.Fatal("failed to connect database")
	}

	db.AutoMigrate(&User{}, &ToDoDeleteFile{}, &ToDoDeleteFileUser{})

	r := mux.NewRouter()
	r.HandleFunc("/api/ToDoDeleteFile", GetToDoDeleteFiles).Methods("GET")
	r.HandleFunc("/api/ToDoDeleteFile", PostToDoDeleteFile).Methods("POST")
	r.HandleFunc("/api/ToDoDeleteFile/execute", RunDeleteFilesJob).Methods("GET")

	log.Fatal(http.ListenAndServe(":5000", r))
}
