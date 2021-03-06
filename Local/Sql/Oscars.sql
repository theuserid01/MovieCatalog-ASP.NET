IF NOT EXISTS (SELECT 1 FROM [dbo].[Oscars])
BEGIN
INSERT INTO Oscars (Category) VALUES
    ('Best Motion Picture of the Year'),
    ('Best Performance by an Actor in a Leading Role'),
    ('Best Performance by an Actress in a Leading Role'),
    ('Best Performance by an Actor in a Supporting Role'),
    ('Best Performance by an Actress in a Supporting Role'),
    ('Best Achievement in Directing'),
    ('Best Writing, Original Screenplay'),
    ('Best Writing, Adapted Screenplay'),
    ('Best Achievement in Cinematography'),
    ('Best Achievement in Costume Design'),
    ('Best Achievement in Sound Mixing'),
    ('Best Achievement in Film Editing'),
    ('Best Achievement in Sound Editing'),
    ('Best Achievement in Visual Effects'),
    ('Best Achievement in Makeup and Hairstyling'),
    ('Best Achievement in Music Written for Motion Pictures, Original Song'),
    ('Best Achievement in Music Written for Motion Pictures, Original Score'),
    ('Best Short Film, Animated'),
    ('Best Short Film, Live Action'),
    ('Best Documentary, Short Subject'),
    ('Best Documentary, Feature'),
    ('Best Foreign Language Film of the Year'),
    ('Best Animated Feature Film of the Year'),
    ('Best Achievement in Production Design'),
    ('Honorary Award'),
    ('Jean Hersholt Humanitarian Award'),
    ('Scientific and Engineering Award'),
    ('Technical Achievement Award')
END
