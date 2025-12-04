CREATE TABLE IF NOT EXISTS "User" (
    Id SERIAL PRIMARY KEY,
    FirstName VARCHAR(100) NOT NULL,
    LastName VARCHAR(100) NOT NULL,
    Avatar VARCHAR(255),
    Email VARCHAR(255) NOT NULL UNIQUE,
    Password VARCHAR(255) NOT NULL,
    RefreshToken VARCHAR(255),
    RefreshTokenExpireTime TIMESTAMP
    );

CREATE INDEX IF NOT EXISTS idx_user_email ON "User"(Email);
