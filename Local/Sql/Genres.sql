IF NOT EXISTS (SELECT 1 FROM [dbo].[Genres])
BEGIN
INSERT INTO Genres (Name) VALUES
    ('Action'),
    ('Adventure'),
    ('Animation'),
    ('Biography'),
    ('Comedy'),
    ('Crime'),
    ('Documentary'),
    ('Drama'),
    ('Family'),
    ('Fantasy'),
    ('Film-Noir'),
    ('History'),
    ('Horror'),
    ('Music'),
    ('Musical'),
    ('Mystery'),
    ('Romance'),
    ('Sci-Fi'),
    ('Shorts'),
    ('Sport'),
    ('Thriller'),
    ('War'),
    ('Western')
END
