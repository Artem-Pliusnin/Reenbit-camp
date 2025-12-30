CREATE TABLE IF NOT EXISTS "CardLabels" (
    Id SERIAL PRIMARY KEY,
    Card_Id INT NOT NULL,
    Label_Id INT NOT NULL,

    CONSTRAINT FK_CardLabels_Card
    FOREIGN KEY (Card_Id)
    REFERENCES "Cards"(Id)
    ON DELETE CASCADE,

    CONSTRAINT FK_CardLabels_Tag
    FOREIGN KEY (Label_Id)
    REFERENCES "Labels"(Id)
    ON DELETE CASCADE,

    CONSTRAINT UQ_Card_Tag
    UNIQUE (Card_Id, Label_Id)
);

CREATE INDEX IF NOT EXISTS idx_cardlabels_card
    ON "CardLabels"(Card_Id);