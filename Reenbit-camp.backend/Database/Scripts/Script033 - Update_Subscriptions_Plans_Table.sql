ALTER TABLE "SubscriptionPlan"
    ADD COLUMN IF NOT EXISTS Is_Ai_Assistant_Available BOOLEAN NOT NULL DEFAULT FALSE,
    ADD COLUMN IF NOT EXISTS Is_Name_Highlighted BOOLEAN NOT NULL DEFAULT FALSE;

UPDATE "SubscriptionPlan"
SET
    Is_Ai_Assistant_Available = TRUE,
    Is_Name_Highlighted = TRUE
WHERE Name = 'Pro';

UPDATE "SubscriptionPlan"
SET
    Is_Ai_Assistant_Available = TRUE
WHERE Name = 'Standard';