UPDATE 'Rover'
SET PosX      = $posX,
    PosY      = $posY,
    Direction = $direction
WHERE Id = $id;