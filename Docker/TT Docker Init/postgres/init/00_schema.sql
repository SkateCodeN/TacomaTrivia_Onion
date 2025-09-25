--Enable UUIDs 
CREATE EXTENSION IF NOT EXISTS pgcrypto;

--Main table (adds day/time)
CREATE TABLE IF NOT EXISTS public.venues (
    id          uuid    PRIMARY KEY DEFAULT gen_random_uuid(),
    name        text    NOT NULL,
    phone       text,
    address     text,
    allowspets  boolean NOT NULL DEFAULT false,
    rounds      integer NOT NULL DEFAULT 0,
    triviaday   smallint,
    triviastart time,
    website     text,
    allowskids  boolean NOT NULL DEFAULT false
);

