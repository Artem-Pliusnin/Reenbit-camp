CREATE TABLE IF NOT EXISTS "BoardMembers" (
     Id SERIAL PRIMARY KEY,

     Board_Id INT NOT NULL
     REFERENCES "Boards"(Id) ON DELETE CASCADE,

    User_Id INT NOT NULL
    REFERENCES "User"(Id) ON DELETE CASCADE,

    User_Role_Id INT NOT NULL DEFAULT 3
    REFERENCES "BoardRoles"(Id) ON DELETE RESTRICT,

    CONSTRAINT UQ_BoardMembers UNIQUE (Board_Id, User_Id)
);

CREATE INDEX IF NOT EXISTS idx_boardmembers_boardid ON "BoardMembers"(Board_Id);
CREATE INDEX IF NOT EXISTS idx_boardmembers_userid ON "BoardMembers"(User_Id);