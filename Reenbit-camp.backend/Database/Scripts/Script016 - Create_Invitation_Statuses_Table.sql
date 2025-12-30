CREATE TABLE IF NOT EXISTS "InvitationStatuses" (
    Id INT PRIMARY KEY,
    Name VARCHAR(50) NOT NULL UNIQUE
);

INSERT INTO "InvitationStatuses" (Id, Name) VALUES
    (1, 'Pending'),
    (2, 'Accepted'),
    (3, 'Declined')
ON CONFLICT DO NOTHING;
