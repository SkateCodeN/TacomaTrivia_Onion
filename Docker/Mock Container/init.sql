-- Enable UUID generation
CREATE EXTENSION IF NOT EXISTS pgcrypto;

-- Create DB objects in the default database (set via POSTGRES_DB)
-- Table: tacoma
CREATE TABLE IF NOT EXISTS tacoma (
  id          uuid PRIMARY KEY DEFAULT gen_random_uuid(),
  name        text        NOT NULL,
  phone       text,
  address     text,
  allowspets  boolean     NOT NULL DEFAULT false,
  rounds      integer     NOT NULL DEFAULT 0
);

-- Seed ~25 rows of filler data
INSERT INTO tacoma (name, phone, address, allowspets, rounds) VALUES
('Harborview Cafe',        '253-555-0101', '101 Pacific Ave, Tacoma, WA 98402', true,  3),
('Ruston Roasters',        '253-555-0102', '2201 Ruston Way, Tacoma, WA 98402', true,  5),
('Grit City Books',        '253-555-0103', '12 Commerce St, Tacoma, WA 98402',  false, 2),
('Union Station Deli',     '253-555-0104', '1717 Pacific Ave, Tacoma, WA 98402', true,  4),
('Stadium Bistro',         '253-555-0105', '24 N Tacoma Ave, Tacoma, WA 98403', false, 1),
('Hilltop Market',         '253-555-0106', '902 MLK Jr Way, Tacoma, WA 98405',  true,  6),
('Pacific Pints',          '253-555-0107', '500 Pacific Ave, Tacoma, WA 98402', true,  7),
('Commencement Donuts',    '253-555-0108', '1401 Dock St, Tacoma, WA 98402',    false, 2),
('Narrows Noodles',        '253-555-0109', '2401 Narrows Dr, Tacoma, WA 98406',  true,  3),
('Wright Park Pizza',      '253-555-0110', '501 S I St, Tacoma, WA 98405',       true,  2),
('Point Defiance Pub',     '253-555-0111', '5400 N Pearl St, Tacoma, WA 98407',  false, 5),
('6th Ave Tacos',          '253-555-0112', '2710 6th Ave, Tacoma, WA 98406',     true,  4),
('Tacoma Game House',      '253-555-0113', '715 St Helens Ave, Tacoma, WA 98402',false, 1),
('Freighthouse BBQ',       '253-555-0114', '2501 N Waterfront Dr, Tacoma, WA',   true,  6),
('Murray Morgan Coffee',   '253-555-0115', '714 Dock St, Tacoma, WA 98402',      true,  3),
('Foss Waterway Sushi',    '253-555-0116', '1205 Dock St, Tacoma, WA 98402',     false, 4),
('Proctor Pasta',          '253-555-0117', '2700 N Proctor St, Tacoma, WA 98407',true,  2),
('McCarver Sandwiches',    '253-555-0118', '2141 S J St, Tacoma, WA 98405',      true,  3),
('Portland Ave Pho',       '253-555-0119', '4502 Portland Ave E, Tacoma, WA',     false, 5),
('Downtown Dumplings',     '253-555-0120', '936 Pacific Ave, Tacoma, WA 98402',   true,  4),
('The Copper Spoon',       '253-555-0121', '6108 6th Ave, Tacoma, WA 98406',      false, 2),
('Glass Museum Grill',     '253-555-0122', '1801 Dock St, Tacoma, WA 98402',      true,  3),
('Titlow Treats',          '253-555-0123', '8425 6th Ave, Tacoma, WA 98465',      true,  6),
('The Blue Mouse Eats',    '253-555-0124', '2611 N Proctor St, Tacoma, WA 98407', false, 1),
('Old Town Bagels',        '253-555-0125', '2201 N 30th St, Tacoma, WA 98403',    true,  2);
