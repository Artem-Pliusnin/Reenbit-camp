CREATE TABLE IF NOT EXISTS "Labels" (
    Id SERIAL PRIMARY KEY,
    Board_Id INT NOT NULL,
    Text VARCHAR(50) NOT NULL,
    Color VARCHAR(9) NOT NULL,

    CONSTRAINT FK_Tags_Board
    FOREIGN KEY (Board_Id)
    REFERENCES "Boards"(Id)
    ON DELETE CASCADE,

    CONSTRAINT CK_Tags_Color_Hex
    CHECK (Color ~ '^#[0-9A-Fa-f]{8}$')
);

CREATE INDEX IF NOT EXISTS idx_labels_board
    ON "Labels"(Board_Id);