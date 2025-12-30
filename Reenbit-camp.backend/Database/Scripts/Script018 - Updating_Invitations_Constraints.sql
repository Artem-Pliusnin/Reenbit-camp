ALTER TABLE "Invitations"
DROP CONSTRAINT IF EXISTS UQ_Board_Invite;

CREATE UNIQUE INDEX IF NOT EXISTS UQ_invitations_board_user_pending
    ON "Invitations"(Board_Id, Invited_User_Id)
    WHERE Status_Id = 1;

ALTER TABLE "Invitations"
    ADD CONSTRAINT chk_invitation_not_self
    CHECK (Invited_User_Id <> Invited_By_User_Id);