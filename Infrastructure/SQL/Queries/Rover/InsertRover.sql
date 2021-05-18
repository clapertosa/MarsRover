INSERT INTO 'Rover'(Posx, PosY, Direction)
VALUES ($posX, $posY, $direction);
SELECT *
FROM 'Rover'
WHERE Id = last_insert_rowid();