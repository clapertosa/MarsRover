﻿DROP TABLE IF EXISTS 'Obstacle';
DROP TABLE IF EXISTS 'Rover';
DROP TABLE IF EXISTS 'Planet';

CREATE TABLE 'Obstacle'(
Id INTEGER PRIMARY KEY,
PosX SMALLINT NOT NULL,
PosY SMALLINT NOT NULL
);

CREATE TABLE 'Rover'(
Id INTEGER PRIMARY KEY,
PosX SMALLINT NOT NULL,
PosY SMALLINT NOT NULL,
Direction CHARACTER(1) NOT NULL
);

CREATE TABLE 'Planet'
(
    Id      INTEGER PRIMARY KEY,
    Name    NVARCHAR(100) NOT NULL,
    Rows    SMALLINT      NOT NULL,
    Columns SMALLINT      NOT NULL,
    RoverId INTEGER       NOT NULL,
    CONSTRAINT FK_Planet_Id FOREIGN KEY (Id) REFERENCES Rover (Id) ON DELETE CASCADE
);