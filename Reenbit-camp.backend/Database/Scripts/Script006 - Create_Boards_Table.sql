CREATE TABLE IF NOT EXISTS "Boards" (
    Id SERIAL PRIMARY KEY,
    Title VARCHAR(255) NOT NULL,
    Created_By INT NOT NULL DEFAULT 1,
    Creation_Date TIMESTAMP NOT NULL,
    Last_Updated_By INT,
    Last_Update_Date TIMESTAMP,

    CONSTRAINT FK_Board_CreatedBy FOREIGN KEY (Created_By)
    REFERENCES "User"(Id)
    ON DELETE SET DEFAULT,

    CONSTRAINT FK_Board_LastUpdatedBy FOREIGN KEY (Last_Updated_By)
    REFERENCES "User"(Id)
    ON DELETE SET NULL
    );
