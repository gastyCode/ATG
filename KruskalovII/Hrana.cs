namespace grafy;

public class Hrana
{
    public int ZaciatocnyVrchol { get; }
    public int KonecnyVrchol { get; }
    public int Cena { get; }

    public Hrana(int zaciatocnyVrchol, int konecnyVrchol, int cena)
    {
        ZaciatocnyVrchol = zaciatocnyVrchol;
        KonecnyVrchol = konecnyVrchol;
        Cena = cena;
    }
}