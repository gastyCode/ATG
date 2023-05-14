namespace grafy;

public class Toky
{
    private Graf _graf;
    private string _nazov;
    private int[] _hodnoty;
    private int _tok;
    private int _uzaver;

    public Toky(string nazov)
    {
        _graf = new Graf($"{nazov}.hrn");
        _nazov = nazov;
        _hodnoty = new int[_graf.PocetVrcholov + 1];
        _uzaver = _graf.PocetVrcholov - 1;
        _tok = 0;
    }

    private bool NajdiZvacsujucuSaPolocestu()
    {
        List<int> _epsilon = new List<int>();
        _epsilon.Add(1);
        for (int i = 0; i < _hodnoty.Length; i++)
        {
            _hodnoty[i] = int.MaxValue;
        }
        _hodnoty[1] = 0;
        while (_hodnoty[_uzaver] == int.MaxValue && _epsilon.Count != 0)
        {
            int vrchol = _epsilon[0];
            _epsilon.RemoveAt(0);
        
            if (_graf.SmernikyVstupne.ContainsKey(vrchol))
            {
                foreach (var hrana in _graf.SmernikyVstupne[vrchol])
                {
                    if (_hodnoty[hrana.ZaciatocnyVrchol] == int.MaxValue && hrana.Tok > 0)
                    {
                        _hodnoty[hrana.ZaciatocnyVrchol] = -vrchol;
                        _epsilon.Add(hrana.ZaciatocnyVrchol);
                    }
                }
            }
        
            if (_graf.SmernikyVystupne.ContainsKey(vrchol))
            {
                foreach (var hrana in _graf.SmernikyVystupne[vrchol])
                {
                    if (_hodnoty[hrana.KonecnyVrchol] == int.MaxValue && hrana.Kapacita - hrana.Tok > 0)
                    {
                        _hodnoty[hrana.KonecnyVrchol] = vrchol;
                        _epsilon.Add(hrana.KonecnyVrchol);
                    }
                }
            }
        }
        return _hodnoty[_uzaver] != int.MaxValue;
    }

    public void NajdiMaximalnyTok()
    {
        while (NajdiZvacsujucuSaPolocestu())
        {
            int rezerva = int.MaxValue;
            int k;
            for (int z = _uzaver;; z = Math.Abs(k))
            {
                k = _hodnoty[z];
                if (k == 0) break;
                if (k > 0)
                {
                    Hrana hrana = _graf.NajdiHranu(k, z);
                    if (rezerva > hrana.Kapacita - hrana.Tok)
                    {
                        rezerva = hrana.Kapacita - hrana.Tok;
                    }
                }
                else
                {
                    Hrana hrana = _graf.NajdiHranu(z, -k);
                    if (rezerva > hrana.Tok)
                    {
                        rezerva = hrana.Tok;
                    }
                }
            }

            for (int z = _uzaver;; z = Math.Abs(k))
            {
                k = _hodnoty[z];
                if (k == 0) break;
                if (k > 0)
                {
                    Hrana hrana = _graf.NajdiHranu(k, z);
                    hrana.Tok += rezerva;
                }
                else
                {
                    Hrana hrana = _graf.NajdiHranu(z, -k);
                    hrana.Tok -= rezerva;
                }
            }
        }

        foreach (var hrana in _graf.SmernikyVystupne[1])
        {
            _tok += hrana.Tok;
        }
        Vypis();
    }

    private void Vypis()
    {
        Console.WriteLine(_nazov);
        Console.WriteLine("----------------");
        Console.WriteLine($"Pocet vrcholov: {_graf.PocetVrcholov}");
        Console.WriteLine($"Pocet hran: {_graf.PocetHran}");
        Console.WriteLine("Toky na hranach siete:");
        foreach (var hrana in _graf.Hrany)
        {
            Console.WriteLine($"({hrana.ZaciatocnyVrchol}, {hrana.KonecnyVrchol}): {hrana.Tok}");
        }
        Console.WriteLine($"Maximalny tok: {_tok}");
    }
}