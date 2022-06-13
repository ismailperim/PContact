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
	PRIMARY KEY (id),
	CONSTRAINT fk_id_person_id FOREIGN KEY (person_id)
        REFERENCES public.person (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
        NOT VALID
);

-- Report table creating
CREATE TABLE IF NOT EXISTS report (
	id UUID NOT NULL,
	location VARCHAR(100),
	status SMALLINT,
	path VARCHAR(100),
	create_date TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
	PRIMARY KEY (id)
);

CREATE INDEX IF NOT EXISTS fki_fk_id_person_id
    ON public.contact USING btree
    (person_id ASC NULLS LAST)
    TABLESPACE pg_default;
    
	
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

-- AddContactInfo function creating
CREATE OR REPLACE FUNCTION public.sp_add_contact_info(
	p_person_id UUID,
	p_type SMALLINT,
	p_value VARCHAR)
    RETURNS UUID
AS $$
DECLARE 
    v_contact_id UUID;
BEGIN

    SELECT uuid_generate_v4() INTO v_contact_id;
    
    INSERT INTO public.contact(id,person_id,type,value)
    SELECT v_contact_id, p_person_id AS person_id, p_type AS type, p_value AS value;
    
    
    RETURN v_contact_id;
END;    
$$ LANGUAGE plpgsql;


-- GetAllPersons function creating
CREATE OR REPLACE FUNCTION public.sp_get_all_persons(p_page_row_count INT DEFAULT 10, p_page_number INT DEFAULT 0)
    RETURNS TABLE(id UUID,name VARCHAR,surname VARCHAR,company VARCHAR,create_date TIMESTAMP WITH TIME ZONE)
AS $$
BEGIN
    RETURN QUERY
    SELECT * FROM public.person
    ORDER BY create_date DESC
	LIMIT p_page_row_count OFFSET (p_page_number * p_page_row_count);
END;    
$$ LANGUAGE plpgsql;

-- GetPersonByID function creating
CREATE OR REPLACE FUNCTION public.sp_get_person_by_id(p_person_id UUID)
    RETURNS TABLE(id UUID,name VARCHAR,surname VARCHAR,company VARCHAR,create_date TIMESTAMP WITH TIME ZONE, contact_info JSON)
AS $$
BEGIN
    RETURN QUERY
    SELECT p.*, ci.data AS contact_info FROM 
    person AS p 
    LEFT JOIN LATERAL (
        SELECT 
            json_agg(json_build_object('ID',c.id,'Type',c.type,'Value',c.value)) AS data,
            c.person_id
        FROM 
            contact c
        GROUP BY person_id) AS ci ON ci.person_id = p.id
    WHERE p.id = p_person_id;
END;    
$$ LANGUAGE plpgsql;

-- Removes a person by ID
CREATE OR REPLACE FUNCTION public.sp_remove_person(p_person_id UUID)
    RETURNS VOID
AS $$
BEGIN
    DELETE FROM public.contact WHERE person_id = p_person_id;
    DELETE FROM public.person WHERE id = p_person_id;
END;    
$$ LANGUAGE plpgsql;

-- Removes a Contact Info from Person
CREATE OR REPLACE FUNCTION public.sp_remove_contact_info(p_person_id UUID, p_contact_info_id UUID)
    RETURNS VOID
AS $$
BEGIN
    DELETE FROM public.contact WHERE id = p_contact_info_id AND person_id = p_person_id;
END;    
$$ LANGUAGE plpgsql;

-- Gets Contact Report Result
CREATE OR REPLACE FUNCTION public.sp_get_contact_report(p_location VARCHAR)
    RETURNS TABLE(location VARCHAR, person_count INT, phone_count INT)
AS $$
DECLARE v_person_count INT;
DECLARE v_phone_count INT;
BEGIN
    SELECT COUNT(1) FROM person p INNER JOIN contact c ON p.id = c.person_id AND c.type = 3 AND c.value = p_location INTO v_person_count;
    SELECT COUNT(1) FROM contact p INNER JOIN contact c ON p.person_id = c.person_id AND c.type = 3 AND c.value = p_location WHERE p.type = 1 INTO v_phone_count;
    RETURN QUERY
    SELECT p_location, v_person_count, v_phone_count;
END;    
$$ LANGUAGE plpgsql;


-- AddReportRequest function creating
CREATE OR REPLACE FUNCTION public.sp_add_report_request(p_location VARCHAR)
    RETURNS UUID
AS $$
DECLARE 
    v_report_id UUID;
BEGIN
    SELECT uuid_generate_v4() INTO v_report_id;

    INSERT INTO public.report(id,location,status)
    VALUES (v_report_id,p_location,1);
    
    RETURN v_report_id;
END;    
$$ LANGUAGE plpgsql;

-- GetAllReports function creating 
CREATE OR REPLACE FUNCTION public.sp_get_all_reports(p_page_row_count INT DEFAULT 10, p_page_number INT DEFAULT 0)
    RETURNS TABLE(id UUID,location VARCHAR,status SMALLINT,path VARCHAR,create_date TIMESTAMP WITH TIME ZONE)
AS $$
BEGIN
    RETURN QUERY
    SELECT * FROM public.report
    ORDER BY create_date DESC
	LIMIT p_page_row_count OFFSET (p_page_number * p_page_row_count);
END;    
$$ LANGUAGE plpgsql;

-- GetReportByID function creating 
CREATE OR REPLACE FUNCTION public.sp_get_report_by_id(p_report_id UUID)
    RETURNS TABLE(id UUID,location VARCHAR,status SMALLINT,path VARCHAR,create_date TIMESTAMP WITH TIME ZONE)
AS $$
BEGIN
    RETURN QUERY
    SELECT * FROM public.report AS r
    WHERE r.id = p_report_id;
END;    
$$ LANGUAGE plpgsql;
