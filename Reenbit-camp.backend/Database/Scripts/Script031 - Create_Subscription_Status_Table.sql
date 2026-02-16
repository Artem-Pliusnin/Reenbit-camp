CREATE TABLE IF NOT EXISTS "SubscriptionStatuses"(
    Id SMALLINT PRIMARY KEY,
    Name VARCHAR(50) NOT NULL UNIQUE
);

INSERT INTO "SubscriptionStatuses"(Id, Name)
VALUES
    (1, 'Active'),
    (2, 'PastDue'),
    (3, 'Canceled'),
    (4, 'Unpaid')
    ON CONFLICT (Id) DO NOTHING;