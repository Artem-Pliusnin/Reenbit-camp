INSERT INTO "BoardStatuses"(Id, Name)
VALUES
    (4, 'Restoring')
    ON CONFLICT (Id) DO NOTHING;