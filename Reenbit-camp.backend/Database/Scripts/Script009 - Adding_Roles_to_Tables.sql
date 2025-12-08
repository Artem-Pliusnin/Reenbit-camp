TRUNCATE TABLE "Roles" RESTART IDENTITY CASCADE;

INSERT INTO "Roles" (Id, Name) VALUES
(1, 'User'),
(2, 'Admin')
ON CONFLICT DO NOTHING;

INSERT INTO "BoardRoles" (Id, Name) VALUES
(1, 'Owner'),
(2, 'Admin'),
(3, 'Member'),
(4, 'Viewer')
ON CONFLICT DO NOTHING;