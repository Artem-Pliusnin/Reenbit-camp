CREATE OR REPLACE PROCEDURE move_list_in_board(
    p_board_id INT,
    p_list_id INT,
    p_new_position INT
)
LANGUAGE plpgsql
AS $$
DECLARE 
    old_position INT;
    max_position INT;
BEGIN
    SELECT position_index into old_position
    FROM "Lists"
    WHERE id=p_list_id AND board_id = p_board_id;

    IF old_position IS NULL THEN
        RAISE EXCEPTION 'List % not found in board %', p_list_id, p_board_id;
    END IF;
    
    IF p_new_position <= 0 THEN
       p_new_position := 1;
    END IF;
    
    SELECT MAX(position_index) INTO max_position
    FROM "Lists"
    WHERE board_id = p_board_id;

    IF p_new_position > max_position THEN
       p_new_position := max_position;
    END IF;
    
    IF p_new_position = old_position THEN
       RETURN;
    END IF;

    IF p_new_position < old_position THEN
       UPDATE "Lists"
       SET position_index = position_index + 1
       WHERE board_id = p_board_id 
         AND position_index >= p_new_position
         AND position_index < old_position;
    END IF;
    
    IF p_new_position > old_position THEN
        UPDATE "Lists"
        SET position_index = position_index - 1
        WHERE board_id = p_board_id
        AND position_index <= p_new_position
        AND position_index > old_position;
    END IF;
    
    UPDATE "Lists"
    SET position_index = p_new_position
    WHERE id = p_list_id;
END;
$$;
    
    
    
    
    