INSERT INTO Planet(Name, Size)
VALUES ($name, $size);
SELECT Id, Name, Size
FROM Planet
WHERE Id = last_insert_rowid();