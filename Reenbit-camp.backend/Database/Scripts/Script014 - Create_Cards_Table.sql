CREATE TABLE IF NOT EXISTS "Cards" (
    Id SERIAL PRIMARY KEY,
    List_Id INT NOT NULL,
    Title VARCHAR(255) NOT NULL,
    Description TEXT,
    Is_Completed BOOLEAN NOT NULL DEFAULT FALSE,
    Position_Index INT NOT NULL,
    Start_Date TIMESTAMPTZ,
    Due_Date TIMESTAMPTZ,
    Last_Updated_By INT,
    Last_Update_Date TIMESTAMPTZ DEFAULT NOW(),

    CONSTRAINT FK_Card_List
    FOREIGN KEY (List_Id)
    REFERENCES "Lists"(Id)
    ON DELETE CASCADE,

    CONSTRAINT FK_Card_LastUpdatedBy
    FOREIGN KEY (Last_Updated_By)
    REFERENCES "User"(Id)
    ON DELETE SET NULL,

    CONSTRAINT uq_card_position
    UNIQUE (List_Id, Position_Index)
    DEFERRABLE INITIALLY DEFERRED
    );
