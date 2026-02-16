CREATE TABLE IF NOT EXISTS "UserSubscriptions"(
    Id SERIAL PRIMARY KEY,
    User_Id INT NOT NULL,
    Subscription_Plan_Id INT NOT NULL,
    Stripe_Customer_Id VARCHAR(255),
    Stripe_Subscription_Id VARCHAR(255),
    Status_Id SMALLINT NOT NULL,
    Current_Period_End TIMESTAMP,
    Cancel_At_Period_End BOOLEAN NOT NULL DEFAULT FALSE,

    CONSTRAINT fk_usersubscription_user
    FOREIGN KEY (User_Id)
    REFERENCES "User"(Id)
    ON DELETE CASCADE,

    CONSTRAINT fk_usersubscription_plan
    FOREIGN KEY (Subscription_Plan_Id)
    REFERENCES "SubscriptionPlan"(Id),

    CONSTRAINT fk_usersubscription_status
    FOREIGN KEY (Status_Id)
    REFERENCES "SubscriptionStatuses"(Id)
    );

CREATE UNIQUE INDEX IF NOT EXISTS ux_usersubscription_user
    ON "UserSubscriptions"(User_Id);