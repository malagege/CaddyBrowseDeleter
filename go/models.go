package main

import (
	"gorm.io/gorm"
)

type User struct {
	gorm.Model
	Name             string
	ToDoDeleteFiles  []*ToDoDeleteFile `gorm:"many2many:to_do_delete_file_user;"`
}

type ToDoDeleteFile struct {
	gorm.Model
	FilePath        string
	DirPath         string
	IsReadyToDelete bool `gorm:"-"`
	Users           []*User `gorm:"many2many:to_do_delete_file_user;"`
}

type ToDoDeleteFileUser struct {
	ToDoDeleteFileID uint
	UserID           uint
}
