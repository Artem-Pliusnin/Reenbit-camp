ALTER TABLE "Lists"
DROP CONSTRAINT IF EXISTS uq_list_position;

ALTER TABLE "Lists"
ADD CONSTRAINT uq_list_position
UNIQUE (board_id, position_index)
DEFERRABLE INITIALLY DEFERRED;
