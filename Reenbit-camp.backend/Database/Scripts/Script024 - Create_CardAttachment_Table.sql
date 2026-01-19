CREATE TABLE IF NOT EXISTS "CardAttachment" (
    Id SERIAL PRIMARY KEY,
    Card_Id INT NOT NULL,
    File_url TEXT NOT NULL,
    File_name VARCHAR(255) NOT NULL,
    Content_type VARCHAR(255) NOT NULL,
    Created_At TIMESTAMPTZ NOT NULL DEFAULT NOW(),
      
    CONSTRAINT FK_CardAttachment_Card
    FOREIGN KEY (Card_Id)
    REFERENCES "Cards"(Id)
    ON DELETE CASCADE
);

CREATE INDEX idx_cardAttachment_Card_id
    ON "CardAttachment" (Card_Id);