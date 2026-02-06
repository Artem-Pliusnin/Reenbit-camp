ALTER TABLE "Boards"
    ADD COLUMN IF NOT EXISTS Status_Id INT NOT NULL DEFAULT 1;

ALTER TABLE "Boards"
    ADD CONSTRAINT FK_Board_Status
    FOREIGN KEY (Status_Id)
    REFERENCES "BoardStatuses"(Id);

CREATE INDEX IF NOT EXISTS idx_boards_archive_status
    ON "Boards"(Status_Id);