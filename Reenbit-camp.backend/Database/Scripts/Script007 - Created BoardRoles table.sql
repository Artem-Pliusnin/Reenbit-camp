CREATE TABLE IF NOT EXISTS "BoardRoles"(
    Id INT PRIMARY KEY,
    Name VARCHAR(50) NOT NULL UNIQUE
    );

INSERT INTO "Roles" (Id, Name) VALUES
(1, 'Owner'),
(2, 'Admin'),
(3, 'Member'),
(4, 'Viewer')
ON CONFLICT DO NOTHING;