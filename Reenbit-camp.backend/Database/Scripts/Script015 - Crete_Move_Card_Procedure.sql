CREATE OR REPLACE PROCEDURE move_card(
    p_card_id INT,
    p_new_list_id INT,
    p_new_position INT
)
LANGUAGE plpgsql
AS $$
DECLARE
    old_list_id INT;
    old_position INT;
    max_position INT;
BEGIN
    SELECT list_id, position_index
    INTO old_list_id, old_position
    FROM "Cards"
    WHERE id = p_card_id;

    IF old_list_id IS NULL THEN
        RAISE EXCEPTION 'Card % not found', p_card_id;
    END IF;
    
    IF NOT EXISTS (
        SELECT 1
        FROM "Lists"
        WHERE id = p_new_list_id
    ) THEN
        p_new_list_id := old_list_id;
    END IF;

    IF p_new_position <= 0 THEN
        p_new_position := 1;
    END IF;

    SELECT COALESCE(MAX(position_index), 0)
    INTO max_position
    FROM "Cards"
    WHERE list_id = p_new_list_id;

    IF p_new_position > max_position + 1 THEN
        p_new_position := max_position + 1;
    END IF;

    IF p_new_list_id = old_list_id THEN
        IF p_new_position = old_position THEN
            RETURN;
        END IF;

        IF p_new_position < old_position THEN
            UPDATE "Cards"
            SET position_index = position_index + 1
            WHERE list_id = old_list_id
            AND position_index >= p_new_position
            AND position_index < old_position;
        ELSE
            UPDATE "Cards"
            SET position_index = position_index - 1
            WHERE list_id = old_list_id
            AND position_index <= p_new_position
            AND position_index > old_position;
        END IF;

        UPDATE "Cards"
        SET position_index = p_new_position
        WHERE id = p_card_id;

        RETURN;
    END IF;


    UPDATE "Cards"
    SET position_index = position_index - 1
    WHERE list_id = old_list_id
    AND position_index > old_position;

    UPDATE "Cards"
    SET position_index = position_index + 1
    WHERE list_id = p_new_list_id
    AND position_index >= p_new_position;

    UPDATE "Cards"
    SET list_id = p_new_list_id,
    position_index = p_new_position
    WHERE id = p_card_id;

END;
$$;
