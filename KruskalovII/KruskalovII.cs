using System.Diagnostics;
using System.Text;

namespace grafy;

public class KruskalovII
{
    private Graf _graf;
    private List<Hrana> _hranyKostry;
    private Dictionary<int, int> _hodnoty;
    private float _casVypoctu;

    public KruskalovII(string nazov)
    {
        _graf = new Graf($"{nazov}.hrn");
        _hranyKostry = new List<Hrana>();
        _hodnoty = new Dictionary<int, int>();
        _casVypoctu = 0;
        Inicializacia();
    }

    public void NajdiNajlacnejsiuKostru()
    {
        HladanieHran();
        VypisKostry();
    }

    private void Inicializacia()
    {
        for (int i = 1; i <= _graf.Vrcholy.Length; i++)
        {
            _hodnoty[i] = i;
        }
    }

    private void HladanieHran()
    {
        Stopwatch casovac = new Stopwatch();
        casovac.Start();
        foreach (var h in _graf.Hrany)
        {
            var z = Najdi(h.ZaciatocnyVrchol);
            var k = Najdi(h.KonecnyVrchol);
            
            if (z == k) continue;
            
            _hodnoty[z] = k;
            _hranyKostry.Add(h);
        }
        casovac.Stop();
        _casVypoctu = casovac.ElapsedMilliseconds;
    }

    private int Najdi(int vrchol)
    {
        if (_hodnoty[vrchol] == vrchol)
        {
            return vrchol;
        }

        _hodnoty[vrchol] = Najdi(_hodnoty[vrchol]);
        return _hodnoty[vrchol];
    }
    
    private void VypisKostry()
    {
        int cena = 0;
        StringBuilder hrany = new StringBuilder();
        
        Console.WriteLine("Hrany v kostre:");
        foreach (var hrana in _hranyKostry)
        {
            hrany.Append($"{{{hrana.ZaciatocnyVrchol}, {hrana.KonecnyVrchol}}} - {hrana.Cena}\n");
            cena += hrana.Cena;
        }
        Console.WriteLine(hrany);
        Console.WriteLine($"Pocet hran: {_hranyKostry.Count}");
        Console.WriteLine($"Cena: {cena}");
        Console.WriteLine($"Cas vypoctu: {_casVypoctu} ms");
    }
}