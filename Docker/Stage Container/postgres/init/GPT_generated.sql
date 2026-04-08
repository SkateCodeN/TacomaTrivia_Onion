-- venues_complete.sql
-- Creates public.venues from scratch and loads all trivia venue data.
-- Safe to re-run: drops and recreates the table each time.
-- triviaday: 0=Sun 1=Mon 2=Tue 3=Wed 4=Thu 5=Fri 6=Sat NULL=unknown

-- ============================================================
-- 1) DROP & CREATE
-- ============================================================

DROP TABLE IF EXISTS public.venues;

CREATE TABLE public.venues (
  id            SERIAL        PRIMARY KEY,
  name          TEXT          NOT NULL,
  phone         TEXT,
  address       TEXT,
  allowspets    BOOLEAN       NOT NULL DEFAULT false,
  allowskids    BOOLEAN       NOT NULL DEFAULT false,
  rounds        INTEGER       NOT NULL DEFAULT 0,
  triviaday     SMALLINT      CHECK (triviaday BETWEEN 0 AND 6),
  triviastart   TIME,
  website       TEXT,
  created_at    TIMESTAMPTZ   NOT NULL DEFAULT now()
);

-- ============================================================
-- 2) INSERT ALL VENUES
-- ============================================================

INSERT INTO public.venues
  (name, phone, address, allowspets, allowskids, rounds, triviaday, triviastart, website)
VALUES
--  name                            phone             address                                        allowspets  allowskids  rounds  triviaday  triviastart   website
  ('Black Star Pub & Grill',       '12535356688',    '158 100th St S, Tacoma, WA 98444',            false,      false,      0,      5,         '19:00:00',   'http://www.blackstar.pub/'),
  ('Red Star Taco Bar',            '12535459795',    '454 St Helens Ave, Tacoma, WA 98402',         false,      true,       0,      1,         '19:00:00',   'https://www.redstartacobar.com/'),
  ('O''Malley''s Irish Pub',       '12536279403',    '2403 6th Ave, Tacoma, WA 98406',              false,      false,      0,      1,         '18:30:00',   'https://omalleysirishpubwa.com/'),
  ('Incline Cider House',          '12533271923',    '2115 S C St, Tacoma, WA 98402',               false,      false,      0,      4,         '19:00:00',   'https://www.inclinecider.com/tap-room'),
  ('Rick J''s Restaurant',         '12535078182',    '6805 176th St E, Puyallup, WA 98375',         false,      false,      0,      4,         '19:00:00',   'https://www.ricky-js.com/'),
  ('The Office Bar & Grill',       '12535723222',    '813 Pacific Ave, Tacoma, WA 98402',           false,      false,      0,      NULL,      NULL,         'http://theofficeonpacific.com/'),
  ('Puget Sound Pizza',            '12533384777',    '317 S 7th St, Tacoma, WA 98402',              false,      false,      0,      4,         '19:00:00',   'https://pugetsoundpizza.com/'),
  ('Doyle''s Public House',        '12532727468',    '208 St Helens Ave, Tacoma, WA 98402',         true,       false,      0,      4,         '20:00:00',   'https://doylespublichouse.com/'),
  ('Half Pint Pizza Pub',          '12532722531',    '2710 6th Ave, Tacoma, WA 98406',              false,      false,      0,      4,         '20:00:00',   NULL),
  ('Airport Tavern',               '12532120709',    '5406 S Tacoma Way, Tacoma, WA 98409',         false,      false,      0,      4,         '19:00:00',   'https://www.airporttavern.com/'),
  ('The Loose Wheel Bar & Grill',  '12533011647',    '6108 6th Ave, Tacoma, WA 98406',              false,      false,      0,      4,         '20:00:00',   'https://www.theloosewheel.com/'),
  ('Narrows Brewing Company',      '12533271400',    '9007 S 19th St, Tacoma, WA 98466',            false,      false,      0,      4,         '19:00:00',   'http://www.narrowsbrewing.com/'),
  ('Dusty''s Hideaway',            '12532920106',    '723 E 34th St, Tacoma, WA 98404',             false,      false,      0,      2,         '19:00:00',   'http://dustyshideaway.com/'),
  ('The Forum',                    '12538302151',    '815 Pacific Ave A, Tacoma, WA 98402',         false,      false,      0,      2,         '19:00:00',   'http://www.eatattheforum.com/'),
  ('Tacoma Comedy Club',           '12532827203',    '933 Market St, Tacoma, WA 98402',             false,      false,      0,      2,         '19:00:00',   'http://www.tacomacomedyclub.com/'),
  ('The Redd Dogg',                '12532121174',    '2805 6th Ave, Tacoma, WA 98406',              false,      false,      0,      2,         '18:00:00',   'https://theredddog.com/menu/6th-ave-tacoma-wa/'),
  ('Crown Bar',                    '12532724177',    '2705 6th Ave, Tacoma, WA 98406',              false,      false,      0,      2,         '19:30:00',   NULL),
  ('Black Star Pub & Grill',       '12535356688',    '158 100th St S, Tacoma, WA 98444',            false,      false,      0,      2,         '19:00:00',   'http://www.blackstar.pub/'),
  ('Sig Brewing Company',          '12535036446',    '2534 Tacoma Ave S, Tacoma, WA 98402',         false,      false,      0,      3,         '19:00:00',   'http://www.sigbrewingco.com/'),
  ('Odd Otter',                    '12533271680',    '716 Pacific Ave, Tacoma, WA 98402',           false,      false,      0,      3,         '18:30:00',   'http://oddotterbrewing.com/'),
  ('E9 Firehouse & Gastropub',     '12532723435',    '611 N Pine St, Tacoma, WA 98406',             false,      false,      0,      3,         '19:30:00',   'http://www.ehouse9.com/'),
  ('Blackfleet Brewing',           '12533271641',    '2302 Fawcett Ave, Tacoma, WA 98402',          false,      false,      0,      NULL,      NULL,         'http://www.blackfleetbrewing.com/'),
  ('Topside Bar & Grill',          '12532123690',    '215 Wilkes St, Steilacoom, WA 98388',         false,      false,      0,      NULL,      NULL,         'http://topsidebargrill.com/'),
  ('Off Day',                      '253302-3047',    '3013 6th Ave C, Tacoma, WA 98406',            false,      false,      0,      3,         '19:00:00',   NULL),
  ('Katie Downs',                  '2537560771',     '3211 Ruston Way, Tacoma, WA 98402',           false,      false,      0,      2,         '19:00:00',   'https://katiedowns.com/'),
  ('Farellis Pizza',               NULL,             '176 & Canyon',                                false,      false,      0,      4,         '19:00:00',   NULL),
  ('Berliner Beer Hall',           '2533025714',     '2401 Pacific Ave, Tacoma, WA 98402',          false,      true,       0,      3,         '19:30:00',   'https://www.berlinerbeerhall.com/');

-- ============================================================
-- 3) VERIFY
-- ============================================================

SELECT
  triviaday,
  CASE triviaday
    WHEN 0 THEN 'Sunday'
    WHEN 1 THEN 'Monday'
    WHEN 2 THEN 'Tuesday'
    WHEN 3 THEN 'Wednesday'
    WHEN 4 THEN 'Thursday'
    WHEN 5 THEN 'Friday'
    WHEN 6 THEN 'Saturday'
    ELSE 'TBD'
  END                         AS day_name,
  COUNT(*)                    AS venue_count
FROM public.venues
GROUP BY triviaday
ORDER BY triviaday NULLS LAST;