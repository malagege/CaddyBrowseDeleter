package main

import (
	"log"
	"os"
	"path/filepath"

	"gorm.io/gorm"
)

func DeleteFilesJob() {
	var files []ToDoDeleteFile
	db.Preload("Users").Find(&files)

	var users []User
	db.Find(&users)

	allUserIDs := make(map[uint]bool)
	for _, user := range users {
		allUserIDs[user.ID] = true
	}

	prefixPath := "./extHDD"
	for _, file := range files {
		readyToDelete := true
		for _, user := range users {
			found := false
			for _, fileUser := range file.Users {
				if user.ID == fileUser.ID {
					found = true
					break
				}
			}
			if !found {
				readyToDelete = false
				break
			}
		}

		if readyToDelete {
			filePath := filepath.Join(prefixPath, file.FilePath)
			if _, err := os.Stat(filePath); os.IsNotExist(err) {
				db.Delete(&file)
				log.Printf("File not found: %s", file.FilePath)
				continue
			}

			if err := os.RemoveAll(filePath); err != nil {
				log.Printf("Error deleting file: %s", file.FilePath)
				continue
			}

			db.Delete(&file)
			log.Printf("Deleted: %s", file.FilePath)
		}
	}
}
