DROP TABLE IF EXISTS 'Obstacle';
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
Direction CHARACTER(1) NOT NULL,
PlanetId INTEGER NOT NULL,
CONSTRAINT FK_PlanetId FOREIGN KEY (PlanetId) REFERENCES Planet (Id)
);

CREATE TABLE 'Planet'
(
    Id      INTEGER PRIMARY KEY,
    Name    NVARCHAR(100) NOT NULL,
    Size    SMALLINT      NOT NULL
);