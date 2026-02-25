INSERT INTO "BoardStatuses"(Id, Name)
VALUES
    (5, 'Blocked')
    ON CONFLICT (Id) DO NOTHING;