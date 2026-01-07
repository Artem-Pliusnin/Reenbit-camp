CREATE OR REPLACE PROCEDURE delete_list_and_reorder(
    p_list_id INT,
    p_board_id INT
)
LANGUAGE plpgsql
AS
$$
DECLARE
    v_position INT;
BEGIN
    SELECT position_index
    INTO v_position
    FROM "Lists"
    WHERE id = p_list_id
      AND board_id = p_board_id;
    
    IF v_position IS NULL THEN
                RAISE EXCEPTION
                    'List % does not exist or does not belong to board %',
                    p_list_id, p_board_id;
    END IF;
    
    DELETE FROM "Lists"
    WHERE id = p_list_id;
    
    UPDATE "Lists"
    SET position_index = position_index - 1
    WHERE board_id = p_board_id
      AND position_index > v_position;
END;
$$;
