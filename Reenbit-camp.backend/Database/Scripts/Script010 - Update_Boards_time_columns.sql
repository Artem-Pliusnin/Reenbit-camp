ALTER TABLE "Boards"
    ALTER COLUMN "creation_date" TYPE timestamp with time zone USING "creation_date" AT TIME ZONE 'UTC';
          
ALTER TABLE "Boards" 
    ALTER COLUMN "last_update_date" TYPE timestamp with time zone USING "last_update_date" AT TIME ZONE 'UTC';