CREATE TABLE IF NOT EXISTS "Session" (
    Id SERIAL PRIMARY KEY,
    Token VARCHAR(255) UNIQUE,
    Expires_On TIMESTAMP,
    User_Id INT UNIQUE REFERENCES "User"(Id) ON DELETE CASCADE
);

CREATE INDEX idx_session_token ON "Session"(token);