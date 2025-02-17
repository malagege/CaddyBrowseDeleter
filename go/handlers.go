package main

import (
	"encoding/json"
	"net/http"
	"os"
	"path/filepath"

	"gorm.io/gorm"
)

func GetToDoDeleteFiles(w http.ResponseWriter, r *http.Request) {
	var users []User
	db.Find(&users)

	var files []ToDoDeleteFile
	db.Preload("Users").Find(&files)

	for i := range files {
		files[i].IsReadyToDelete = true
		for _, user := range users {
			found := false
			for _, fileUser := range files[i].Users {
				if user.ID == fileUser.ID {
					found = true
					break
				}
			}
			if !found {
				files[i].IsReadyToDelete = false
				break
			}
		}
	}

	json.NewEncoder(w).Encode(files)
}

func PostToDoDeleteFile(w http.ResponseWriter, r *http.Request) {
	var input struct {
		FilePath string
		UserName string
	}
	json.NewDecoder(r.Body).Decode(&input)

	var user User
	if err := db.Where("name = ?", input.UserName).First(&user).Error; err != nil {
		http.Error(w, "使用者不存在", http.StatusBadRequest)
		return
	}

	var file ToDoDeleteFile
	if err := db.Preload("Users").Where("file_path = ?", input.FilePath).First(&file).Error; err == nil {
		for i, fileUser := range file.Users {
			if fileUser.ID == user.ID {
				file.Users = append(file.Users[:i], file.Users[i+1:]...)
				db.Save(&file)
				json.NewEncoder(w).Encode(file)
				return
			}
		}
		file.Users = append(file.Users, &user)
		db.Save(&file)
		json.NewEncoder(w).Encode(file)
		return
	}

	dirPath := filepath.Dir(input.FilePath)
	file = ToDoDeleteFile{
		FilePath: input.FilePath,
		DirPath:  dirPath,
		Users:    []*User{&user},
	}
	db.Create(&file)
	json.NewEncoder(w).Encode(file)
}

func RunDeleteFilesJob(w http.ResponseWriter, r *http.Request) {
	DeleteFilesJob()
	w.Write([]byte("排程已執行"))
}
