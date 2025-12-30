CREATE TABLE IF NOT EXISTS "Invitations" (
    Id SERIAL PRIMARY KEY,
    
    Board_Id INT NOT NULL,
    Invited_User_Id INT NOT NULL,
    Invited_By_User_Id INT,
    
    Status_Id INT NOT NULL DEFAULT 1,
    
    Created_At TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    Responded_At TIMESTAMPTZ NULL,

    CONSTRAINT FK_Board
    FOREIGN KEY (Board_Id)
    REFERENCES "Boards"(Id)
    ON DELETE CASCADE,

    CONSTRAINT FK_Invited_User
    FOREIGN KEY (Invited_User_Id)
    REFERENCES "User"(Id)
    ON DELETE CASCADE,

    CONSTRAINT FK_Invited_By_User
    FOREIGN KEY (Invited_By_User_Id)
    REFERENCES "User"(Id)
    ON DELETE SET NULL,

    CONSTRAINT FK_Status
    FOREIGN KEY (Status_Id)
    REFERENCES "InvitationStatuses"(id),

    CONSTRAINT UQ_Board_Invite
    UNIQUE (Board_Id, Invited_User_Id)
    );

CREATE INDEX IF NOT EXISTS idx_board_invitations_invited_user
    ON "Invitations"(Invited_User_Id);

CREATE INDEX IF NOT EXISTS idx_board_invitations_status
    ON "Invitations"(Status_Id);