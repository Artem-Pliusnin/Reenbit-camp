CREATE TABLE IF NOT EXISTS "CardMembers" (
    Id SERIAL PRIMARY KEY,
    Card_Id INT NOT NULL,
    User_Id INT NOT NULL,

    CONSTRAINT FK_CardLabels_Card
    FOREIGN KEY (Card_Id)
    REFERENCES "Cards"(Id)
    ON DELETE CASCADE,

    CONSTRAINT FK_CardLabels_Tag
    FOREIGN KEY (User_Id)
    REFERENCES "User"(Id)
    ON DELETE CASCADE,

    CONSTRAINT UQ_Card_Member
    UNIQUE (Card_Id, User_Id)
    );

CREATE INDEX IF NOT EXISTS idx_cardmembers_card
    ON "CardMembers"(Card_Id);