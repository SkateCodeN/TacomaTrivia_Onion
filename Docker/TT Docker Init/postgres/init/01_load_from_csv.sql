-- Loads /docker-entrypoint-initdb.d/tacoma.csv into public.venues

-- 1) Staging table shaped to CSV header (13 columns)
DROP TABLE IF EXISTS public.tacoma_staging;
CREATE TABLE public.tacoma_staging (
  name                text,
  day                 text,
  time                text,
  "duration estimate" text,
  distance            text,
  address             text,
  phone               text,
  website             text,
  pups                text,
  kiddos              text,
  c10                 text,
  c11                 text,
  c12                 text
);

-- 2) COPY from CSV (blank -> NULL)
COPY public.tacoma_staging
  (name, day, time, "duration estimate", distance, address, phone, website, pups, kiddos, c10, c11, c12)
FROM '/docker-entrypoint-initdb.d/tacoma.csv'
WITH (FORMAT csv, HEADER true, NULL '');

-- 3) Transform + insert (robust day/time parsing; ambiguous times default to PM)
WITH s AS (
  SELECT
    btrim(name)                                  AS name,
    lower(btrim(day))                            AS day,
    btrim(time)                                  AS time_text,
    NULLIF(btrim(address), '')                   AS address,
    NULLIF(btrim(phone),   '')                   AS phone,
    NULLIF(btrim(website), '')                   AS website,
    lower(btrim(COALESCE(pups,   '')))           AS pups,
    lower(btrim(COALESCE(kiddos, '')))           AS kiddos
  FROM public.tacoma_staging
),
t AS (
  SELECT
    name, phone, address, website, pups, kiddos,
    CASE
      WHEN day IN ('sun','sunday')                  THEN 0
      WHEN day IN ('mon','monday')                  THEN 1
      WHEN day IN ('tue','tues','tuesday')          THEN 2
      WHEN day IN ('wed','wednesday')               THEN 3
      WHEN day IN ('thu','thur','thurs','thursday') THEN 4
      WHEN day IN ('fri','friday')                  THEN 5
      WHEN day IN ('sat','saturday')                THEN 6
      ELSE NULL
    END AS triviaday,
    CASE
      WHEN time_text IS NULL OR btrim(time_text) = '' OR time_text IN ('?','tbd') THEN NULL

      -- 12h with minutes + AM/PM: "7:00 PM", "07:30 am"
      WHEN time_text ~* '^[0-9]{1,2}:[0-9]{2}[[:space:]]*(AM|PM)$'
        THEN to_timestamp(time_text, 'HH12:MI AM')::time

      -- 12h without minutes: "7 PM", "7pm" -> assume :00
      WHEN time_text ~* '^[0-9]{1,2}[[:space:]]*(AM|PM)$'
        THEN to_timestamp(regexp_replace(time_text, '[[:space:]]*(AM|PM)$', ':00 \1', 1, 1, 'i'), 'HH12:MI AM')::time

      -- 24h or bare "H:MM"/"HH:MM" (e.g., "19:00", "7:30", "07:30")
      WHEN time_text ~* '^[0-9]{1,2}:[0-9]{2}$'
        THEN make_time(
               CASE
                 -- treat bare 1..11 as PM (e.g., "7:30" -> 19:30)
                 WHEN split_part(time_text,':',1)::int BETWEEN 1 AND 11
                   THEN split_part(time_text,':',1)::int + 12
                 ELSE split_part(time_text,':',1)::int
               END,
               split_part(time_text,':',2)::int,
               0
             )

      ELSE NULL
    END AS triviastart,
    CASE
      WHEN pups LIKE 'service%' THEN false
      WHEN pups IN ('y','yes','true','1') THEN true
      WHEN pups IN ('n','no','false','0') THEN false
      ELSE false
    END AS allowspets,
    CASE
      WHEN kiddos IN ('y','yes','true','1') THEN true
      WHEN kiddos IN ('n','no','false','0') THEN false
      ELSE false
    END AS allowskids
  FROM s
)
INSERT INTO public.venues
  (name, phone, address, allowspets, rounds, triviaday, triviastart, website, allowskids)
SELECT
  name,
  phone,
  address,
  allowspets,
  0 AS rounds,
  triviaday,
  triviastart,
  website,
  allowskids
FROM t
WHERE name IS NOT NULL;

-- Helpful counts
SELECT
  (SELECT COUNT(*) FROM public.tacoma_staging) AS staging_rows,
  (SELECT COUNT(*) FROM public.venues)         AS venues_rows;
