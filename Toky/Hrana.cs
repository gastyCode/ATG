namespace grafy;

public class Hrana
{
    public int ZaciatocnyVrchol { get; }
    public int KonecnyVrchol { get; }
    public int Cena { get; }
    public int Kapacita { get; }
    public int Tok { get; set; }

    public Hrana(int zaciatocnyVrchol, int konecnyVrchol, int cena, int kapacita)
    {
        ZaciatocnyVrchol = zaciatocnyVrchol;
        KonecnyVrchol = konecnyVrchol;
        Cena = cena;
        Kapacita = kapacita;
        Tok = 0;
    }
}