INSERT INTO Planet(Name, Rows, Columns)
VALUES ($name, $rows, $columns);
SELECT Id, Name, Rows, Columns
FROM Planet
WHERE Id = last_insert_rowid();