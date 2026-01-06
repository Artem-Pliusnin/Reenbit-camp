CREATE TABLE IF NOT EXISTS "Comments" (
    Id SERIAL PRIMARY KEY,
    Card_Id INT NOT NULL,
    User_Id INT NOT NULL,
    Text TEXT NOT NULL,
    Is_Edited BOOLEAN NOT NULL DEFAULT FALSE,
    Created_At TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    
    CONSTRAINT FK_CardComments_Card
    FOREIGN KEY (Card_Id)
    REFERENCES "Cards"(Id)
    ON DELETE CASCADE,

    CONSTRAINT FK_CardComments_User
    FOREIGN KEY (User_Id)
    REFERENCES "User"(Id)
    ON DELETE CASCADE
);

CREATE INDEX idx_cardCommentsCard_id
    ON "Comments" (Card_Id);
