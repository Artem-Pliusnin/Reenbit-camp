CREATE OR REPLACE PROCEDURE delete_card_and_reorder(
    p_card_id INT,
    p_list_id INT
)
LANGUAGE plpgsql
AS
$$
DECLARE
    v_position INT;
BEGIN
    SELECT position_index
    INTO v_position
    FROM "Cards"
    WHERE id = p_card_id
      AND list_id = p_list_id;
    
    IF v_position IS NULL THEN
            RAISE EXCEPTION
                'Card % does not exist or does not belong to list %',
                p_card_id, p_list_id;
    END IF;
    
    DELETE FROM "Cards"
    WHERE id = p_card_id;
    
    UPDATE "Cards"
    SET position_index = position_index - 1
    WHERE list_id = p_list_id
    AND position_index > v_position;
END;
$$;
