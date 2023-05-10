using System.Collections;
using System.Text;

namespace grafy;

public class CPM
{
    private Graf _graf;
    private int[] _zaciatky;
    private int[] _konce;
    private int[] _rezervy;
    private int[] _maximalneTrvanie;
    private int _trvanie;
    private List<int> _kritickeVrcholy;

    public CPM(string nazov)
    {
        _graf = new Graf(nazov);

        _zaciatky = new int[_graf.VrcholyMonotonne.Length];
        _konce = new int[_graf.VrcholyMonotonne.Length];
        _rezervy = new int[_graf.VrcholyMonotonne.Length];
        _maximalneTrvanie = new int[_graf.VrcholyMonotonne.Length];
        _trvanie = 0;
        _kritickeVrcholy = new List<int>();
    }

    public void Vykonaj()
    {
        for (var i = 0; i < _graf.VrcholyMonotonne.Length; i++)
        {
            foreach (var hrana in _graf.SmernikyVystupne[_graf.VrcholyMonotonne[i].Index])
            {
                if (_zaciatky[hrana.KonecnyVrchol - 1] < _zaciatky[hrana.ZaciatocnyVrchol - 1] + _graf.VrcholyMonotonne[i].Trvanie)
                {
                    _zaciatky[hrana.KonecnyVrchol - 1] = _zaciatky[hrana.ZaciatocnyVrchol - 1] + _graf.VrcholyMonotonne[i].Trvanie;
                }
            }
        }

        for (int i = 0; i < _konce.Length; i++)
        {
            _trvanie = Math.Max(_trvanie, _zaciatky[i] + _graf.Vrcholy[i].Trvanie);
        }

        for (int i = 0; i < _konce.Length; i++)
        {
            _konce[i] = _trvanie;
        }

        for (int i = _graf.VrcholyMonotonne.Length - 1; i >= 0; i--)
        {
            if (!_graf.SmernikyVstupne.ContainsKey(_graf.VrcholyMonotonne[i].Index)) continue;
            foreach (var hrana in _graf.SmernikyVstupne[_graf.VrcholyMonotonne[i].Index])
            {
                if (_konce[hrana.ZaciatocnyVrchol - 1] > _konce[hrana.KonecnyVrchol - 1] - _graf.VrcholyMonotonne[i].Trvanie)
                {
                    _konce[hrana.ZaciatocnyVrchol - 1] = _konce[hrana.KonecnyVrchol - 1] - _graf.VrcholyMonotonne[i].Trvanie;
                }
            }
        }
        
        for (int i = 0; i < _maximalneTrvanie.Length; i++)
        {
            _maximalneTrvanie[i] = _konce[i] - _zaciatky[i];
            _rezervy[i] = _maximalneTrvanie[i] - _graf.Vrcholy[i].Trvanie;
            if (_rezervy[i] == 0)
            {
                _kritickeVrcholy.Add(i + 1);
            }
        }
        
        VypisCpm();
    }

    private void VypisCpm()
    {
        StringBuilder vrcholy = new StringBuilder();
        StringBuilder zaciatky = new StringBuilder();
        StringBuilder konce = new StringBuilder();
        StringBuilder kritickeVrcholy = new StringBuilder();

        vrcholy.Append("| ");
        foreach (Vrchol vrchol in _graf.VrcholyMonotonne)
        {
            vrcholy.Append($"{vrchol.Index,4} | ");
        }

        zaciatky.Append("| ");
        foreach (var i in _zaciatky)
        {
            zaciatky.Append($"{i,4} | ");
        }
        
        konce.Append("| ");
        foreach (var i in _konce)
        {
            konce.Append($"{i,4} | ");
        }
        
        foreach (var i in _kritickeVrcholy)
        {
            kritickeVrcholy.Append($"{i}, ");
        }

        Console.WriteLine(_graf.Nazov);
        Console.WriteLine(String.Concat(Enumerable.Repeat("_", konce.Length - 1)));
        Console.WriteLine(vrcholy);
        Console.WriteLine("Najskor mozne zaciatky:");
        Console.WriteLine(zaciatky);
        Console.WriteLine("Najneskor nutne konce:");
        Console.WriteLine(konce);
        Console.WriteLine(String.Concat(Enumerable.Repeat("_", konce.Length - 1)));
        Console.WriteLine("Kriticke vrcholy su:");
        Console.WriteLine(kritickeVrcholy);
        Console.WriteLine($"Trvanie: {_trvanie}");
    }
}