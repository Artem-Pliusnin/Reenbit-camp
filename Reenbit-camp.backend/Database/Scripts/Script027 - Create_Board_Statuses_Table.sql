CREATE TABLE IF NOT EXISTS "BoardStatuses"(
    Id INT PRIMARY KEY,
    Name VARCHAR(50) NOT NULL UNIQUE
    );

INSERT INTO "BoardStatuses"(Id, Name)
VALUES
    (1, 'Active'),
    (2, 'Pending'),
    (3, 'Archived')
    ON CONFLICT (Id) DO NOTHING;