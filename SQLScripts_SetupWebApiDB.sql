CREATE DATABASE WebApiDB;
GO

USE WebApiDB;
GO

CREATE TABLE Users (
	Id int NOT NULL PRIMARY KEY IDENTITY,
	Email varchar(50) NOT NULL,
	FirstName varchar(50) NOT NULL,
	LastName varchar(50) NOT NULL
);
GO

CREATE TABLE Posts (
	Id int NOT NULL PRIMARY KEY IDENTITY,
	UserId int NOT NULL,
	PostName varchar(50) NOT NULL,
	PostBody varchar(MAX) NOT NULL,
	Time datetime NOT NULL,
	FOREIGN KEY (UserId) REFERENCES Users(Id)
);
GO