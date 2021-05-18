INSERT INTO 'Obstacle'(PosX, PosY)
VALUES ($posX, $posY);
SELECT *
FROM 'Obstacle'
WHERE Id = last_insert_rowid();