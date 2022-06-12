-- GUID Extension creating
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

-- Person table creating
CREATE TABLE IF NOT EXISTS person (
	id UUID NOT NULL,
	name VARCHAR(100),
	surname VARCHAR(100),
	company VARCHAR(100),
	create_date TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
	PRIMARY KEY (id)
);

--  Contact table creating
CREATE TABLE IF NOT EXISTS contact (
	id UUID NOT NULL,
	person_id	UUID NOT NULL,
	type SMALLINT,
	value VARCHAR(100),
	create_date TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
	PRIMARY KEY (id)
);

-- AddPerson function creating
CREATE OR REPLACE FUNCTION public.sp_add_person(p_name VARCHAR,p_surname VARCHAR,p_company VARCHAR,p_contact_infos JSONB DEFAULT NULL)
    RETURNS UUID
AS $$
DECLARE 
    v_person_id UUID;
BEGIN
    SELECT uuid_generate_v4() INTO v_person_id;

    -- Inserting person record to person table
    INSERT INTO public.person(id,name,surname,company)
    VALUES (v_person_id,p_name,p_surname,p_company);
    
    INSERT INTO public.contact(id,person_id,type,value)
    SELECT uuid_generate_v4(), v_person_id AS person_id, (value->'Type')::SMALLINT AS type,(value->>'Value')::VARCHAR AS val FROM jsonb_array_elements(p_contact_infos);
    
    
    RETURN v_person_id;
END;    
$$ LANGUAGE plpgsql;