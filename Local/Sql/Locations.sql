IF NOT EXISTS (SELECT 1 FROM [dbo].[Locations])
BEGIN
INSERT INTO Locations (Name) VALUES
    ('Berlin International Film Festival'),
    ('Cannes Film Festival'),
    ('Los Angeles'),
    ('New York City'),
    ('Sundance Film Festival'),
    ('Toronto International Film Festival'),
    ('Venice Film Festival')
END
