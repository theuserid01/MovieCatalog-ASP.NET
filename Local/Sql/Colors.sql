IF NOT EXISTS (SELECT 1 FROM [dbo].[Colors])
BEGIN
INSERT INTO Colors (Name) VALUES
    ('Black and White'),
    ('Color')
END
