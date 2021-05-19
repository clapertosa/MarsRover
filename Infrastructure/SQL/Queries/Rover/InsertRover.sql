INSERT INTO 'Rover'(Posx, PosY, Direction, PlanetId)
VALUES ($posX, $posY, $direction, $planetId);
SELECT Id, PosX, PosY, Direction, PlanetId
FROM 'Rover'
WHERE Id = last_insert_rowid();