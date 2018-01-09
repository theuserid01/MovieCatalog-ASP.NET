IF NOT EXISTS (SELECT 1 FROM [dbo].[GoldenGlobes])
BEGIN
INSERT INTO GoldenGlobes (Category) VALUES
    ('Best Motion Picture - Drama'),
    ('Best Motion Picture - Animated'),
    ('Best Motion Picture - Comedy or Musical'),
    ('Best Motion Picture - Foreign Language'),
    ('Best Performance by an Actor in a Motion Picture - Drama'),
    ('Best Performance by an Actor in a Motion Picture - Comedy or Musical'),
    ('Best Performance by an Actress in a Motion Picture - Drama'),
    ('Best Performance by an Actress in a Motion Picture - Comedy or Musica'),
    ('Best Performance by an Actor in a Supporting Role in a Motion Picture'),
    ('Best Performance by an Actress in a Supporting Role in a Motion Picture'),
    ('Best Director - Motion Picture'),
    ('Best Screenplay - Motion Picture'),
    ('Best Original Song - Motion Picture'),
    ('Best Original Score - Motion Picture'),
    ('Best Performance by an Actor in a Television Series - Drama'),
    ('Best Performance by an Actor in a Limited Series or Motion Picture Made for Television'),
    ('Best Performance by an Actor in a Television Series - Comedy or Musical'),
    ('Best Performance by an Actress in a Television Series - Drama'),
    ('Best Performance by an Actress in a Television Series - Comedy or Musical'),
    ('Best Performance by an Actress in a Limited Series or Motion Picture Made for Television'),
    ('Best Performance by an Actor in a Supporting Role in a Series, Limited Series or Motion Picture Made for Television'),
    ('Best Performance by an Actress in a Supporting Role in a Series, Limited Series or Motion Picture Made for Television'),
    ('Best Television Series - Comedy or Musical'),
    ('Best Television Series - Drama'),
    ('Best Television Limited Series or Motion Picture Made for Television'),
    ('Cecil B. DeMille Award'),
    ('Miss Golden Globe')
END
