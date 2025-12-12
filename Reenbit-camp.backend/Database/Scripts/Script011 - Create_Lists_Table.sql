CREATE TABLE IF NOT EXISTS "Lists" (
    Id SERIAL PRIMARY KEY,
    Board_Id INT NOT NULL,
    Title VARCHAR(100) NOT NULL,
    Position_Index INT NOT NULL,
    Last_Updated_By INT,
    Last_Update_Date TIMESTAMPTZ DEFAULT NOW(),

    CONSTRAINT FK_Board_Id FOREIGN KEY (Board_Id)
    REFERENCES "Boards"(Id)
    ON DELETE CASCADE,

    CONSTRAINT FK_Board_LastUpdatedBy FOREIGN KEY (Last_Updated_By)
    REFERENCES "User"(Id)
    ON DELETE SET NULL,

    CONSTRAINT UQ_List_Position UNIQUE (Board_Id, Position_index)
    );
