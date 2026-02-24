CREATE TABLE IF NOT EXISTS "SubscriptionPlan"(
    Id SERIAL PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Board_Limit INT NOT NULL CHECK (Board_Limit >= 0),
    Monthly_Price NUMERIC(10,2) NOT NULL CHECK (Monthly_Price >= 0),
    Stripe_Product_Id VARCHAR(255),
    Stripe_Price_Id VARCHAR(255),
    Created_At TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

CREATE UNIQUE INDEX IF NOT EXISTS ux_subscriptionplan_name
    ON "SubscriptionPlan"(Name);

INSERT INTO "SubscriptionPlan"
(Name, Board_Limit, Monthly_Price, Stripe_Product_Id, Stripe_Price_Id)
VALUES
    ('Free', 10, 0, NULL, NULL),
    ('Standard', 50, 5, 'prod_U2LVL1QZK0aoMJ', 'price_1T4GpiR5pv9xzX33LuGYPpKA'),
    ('Pro', 100, 10, 'prod_U2LVL1QZK0aoMJ', 'price_1T4GorR5pv9xzX336XfAPKmX')
ON CONFLICT (Name) DO NOTHING;