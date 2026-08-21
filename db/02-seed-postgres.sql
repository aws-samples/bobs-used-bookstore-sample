-- Seed data for the converted Aurora PostgreSQL schema.
--
-- Why this file exists:
-- The application defines its reference data and book catalogue as EF Core
-- HasData seed data (see app/Bookstore.Data/SeedData.cs). EF only applies
-- HasData through EnsureCreated/migrations, and Bookstore.Web only calls
-- EnsureCreatedAsync when CanConnectAsync fails. Against an Aurora cluster
-- where the converted schema is already deployed, CanConnectAsync succeeds,
-- so the seed data is never written and the catalogue renders empty.
--
-- Two PostgreSQL-specific details are handled here:
--   1. The converted schema declares id columns as GENERATED ALWAYS AS
--      IDENTITY, so inserting explicit ids requires OVERRIDING SYSTEM VALUE.
--   2. Inserting explicit ids does NOT advance the identity sequence. Without
--      the ALTER ... RESTART statements at the end, the first row the
--      application inserts would start at 1 and collide with the seeded rows.
--
-- Safe to re-run: every insert is guarded with ON CONFLICT (id) DO NOTHING.

SET search_path TO bobsusedbookstore_dbo;

-- Reference data. datatype maps to Bookstore.Domain.ReferenceData.ReferenceDataType:
--   Publisher = 0, Condition = 1, BookType = 2, Genre = 3
INSERT INTO referencedata (id, datatype, text, createdby, createdon, updatedon)
OVERRIDING SYSTEM VALUE
VALUES
    (1,  2, 'Hardcover',                    'System', NOW(), NOW()),
    (2,  2, 'Trade Paperback',               'System', NOW(), NOW()),
    (3,  2, 'Mass Market Paperback',         'System', NOW(), NOW()),
    (4,  1, 'New',                           'System', NOW(), NOW()),
    (5,  1, 'Like New',                      'System', NOW(), NOW()),
    (6,  1, 'Good',                          'System', NOW(), NOW()),
    (7,  1, 'Acceptable',                    'System', NOW(), NOW()),
    (8,  3, 'Biographies',                   'System', NOW(), NOW()),
    (9,  3, 'Children''s Books',             'System', NOW(), NOW()),
    (10, 3, 'History',                       'System', NOW(), NOW()),
    (11, 3, 'Literature & Fiction',          'System', NOW(), NOW()),
    (12, 3, 'Mystery, Thriller & Suspense',  'System', NOW(), NOW()),
    (13, 3, 'Science Fiction & Fantasy',     'System', NOW(), NOW()),
    (14, 3, 'Travel',                        'System', NOW(), NOW()),
    (15, 0, 'Arcadia Books',                 'System', NOW(), NOW()),
    (16, 0, 'Astral Publishing',             'System', NOW(), NOW()),
    (17, 0, 'Moonlight Publishing',          'System', NOW(), NOW()),
    (18, 0, 'Dreamscape Press',              'System', NOW(), NOW()),
    (19, 0, 'Enchanted Library',             'System', NOW(), NOW()),
    (20, 0, 'Fantasia House',                'System', NOW(), NOW()),
    (21, 0, 'Horizon Books',                 'System', NOW(), NOW()),
    (22, 0, 'Infinity Press',                'System', NOW(), NOW()),
    (23, 0, 'Paradigm Publishing',           'System', NOW(), NOW()),
    (24, 0, 'Aurora Publishing',             'System', NOW(), NOW())
ON CONFLICT (id) DO NOTHING;

-- Book catalogue.
INSERT INTO book (id, name, author, isbn, publisherid, booktypeid, genreid, conditionid,
                  price, quantity, year, summary, coverimageurl,
                  createdby, createdon, updatedon)
OVERRIDING SYSTEM VALUE
VALUES
    (1, '2020: The Apocalypse',     'Li Juan',        '6556784356', 15, 1, 13, 5, 10.95, 25, NULL, NULL, '/images/coverimages/apocalypse.png',         'System', NOW(), NOW()),
    (2, 'Children Of Iron',         'Nikki Wolf',     '7665438976', 16, 1, 11, 6, 13.95,  3, NULL, NULL, '/images/coverimages/childrenofiron.png',     'System', NOW(), NOW()),
    (3, 'Gold In The Dark',         'Richard Roe',    '5442280765', 17, 1, 13, 5,  6.50, 10, NULL, NULL, '/images/coverimages/goldinthedark.png',      'System', NOW(), NOW()),
    (4, 'Leagues Of Smoke',         'Pat Candella',   '4556789542', 18, 2, 11, 7,  3.00,  1, NULL, NULL, '/images/coverimages/leaguesofsmoke.png',     'System', NOW(), NOW()),
    (5, 'Alone With The Stars',     'Carlos Salazar', '4563358087', 19, 2, 12, 5, 15.95,  5, NULL, NULL, '/images/coverimages/alonewiththestars.png',  'System', NOW(), NOW()),
    (6, 'The Girl In The Polaroid', 'Terri Whitlock', '2354435678', 20, 1, 12, 6,  8.25,  2, NULL, NULL, '/images/coverimages/girlinthepolaroid.png',  'System', NOW(), NOW()),
    (7, '1001 Jokes',               'Mary Major',     '6554789632', 21, 2, 11, 5, 13.95,  7, NULL, NULL, '/images/coverimages/1001jokes.png',          'System', NOW(), NOW()),
    (8, 'My Search For Meaning',    'Mateo Jackson',  '4558786554', 22, 3,  8, 7,  5.00, 15, NULL, NULL, '/images/coverimages/mysearchformeaning.png', 'System', NOW(), NOW())
ON CONFLICT (id) DO NOTHING;

-- Advance the identity sequences past the explicitly inserted ids so that rows
-- created by the application do not collide with the seeded rows.
SELECT setval(pg_get_serial_sequence('bobsusedbookstore_dbo.referencedata', 'id'),
              (SELECT MAX(id) FROM referencedata));
SELECT setval(pg_get_serial_sequence('bobsusedbookstore_dbo.book', 'id'),
              (SELECT MAX(id) FROM book));

-- Verification.
SELECT 'referencedata' AS table_name, COUNT(*) AS row_count FROM referencedata
UNION ALL
SELECT 'book', COUNT(*) FROM book;
