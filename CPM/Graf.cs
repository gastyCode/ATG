using System.Text.RegularExpressions;

namespace grafy
{
    internal class Graf
    {
        public string Nazov { get; }
        public Vrchol[] VrcholyMonotonne { get; }
        public Vrchol[] Vrcholy { get; private set; }
        public Hrana[] Hrany { get; }
        public int PocetVrcholov { get; }
        public int PocetHran { get; }
        public Dictionary<int, List<Hrana>> SmernikyVystupne { get; private set; }
        public Dictionary<int, List<Hrana>> SmernikyVstupne { get; private set; }
        private Dictionary<int, int> _zastupenieVrcholov;
        private int[] _trvanie;

        public Graf(string nazov)
        {
            Nazov = nazov;
            String grafCesta = nazov + ".hrn";
            String casyCesta = nazov + ".tim";
            _zastupenieVrcholov = new Dictionary<int, int>();
            try
            {
                PocetVrcholov = 0;
                if (File.Exists(grafCesta) && File.Exists(casyCesta))
                {

                    string[] grafRiadky = File.ReadAllLines(grafCesta);
                    string[] casyRiadky = File.ReadAllLines(casyCesta);
                    Hrany = new Hrana[grafRiadky.Length];
                    _trvanie = new int[casyRiadky.Length];

                    for (int i = 0; i < grafRiadky.Length; i++)
                    {
                        string opravenyRiadok = Regex.Replace(grafRiadky[i], "\\s+", " ");
                        string[] rozdelenyRiadok = opravenyRiadok.Split(" ");
                        rozdelenyRiadok = rozdelenyRiadok.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                        int[] cisla = Array.ConvertAll(rozdelenyRiadok, x => int.Parse(x));

                        if (!_zastupenieVrcholov.TryAdd(cisla[1], 1)) _zastupenieVrcholov[cisla[1]]++;
                        _zastupenieVrcholov.TryAdd(cisla[0], 0);
                        
                        Hrany[i] = new Hrana(cisla[0], cisla[1], cisla[2]);
                        PocetVrcholov = PocetVrcholov < cisla[0] ? cisla[0] : PocetVrcholov;
                        PocetVrcholov = PocetVrcholov < cisla[1] ? cisla[1] : PocetVrcholov;
                    }

                    PocetHran = grafRiadky.Length;

                    for (int i = 0; i < casyRiadky.Length; i++)
                    {
                        _trvanie[i] = int.Parse(casyRiadky[i]);
                    }
                }

                VrcholyMonotonne = new Vrchol[PocetVrcholov];

                VytvorPoleSmernikovVystupnych();
                VytvorPoleSmernikovVstupnych();
                ZoradMonotonne();
            }
            catch (FileNotFoundException e)
            {
                Console.Error.WriteLine(e);
            }
        }

        private void ZoradMonotonne()
        {
            List<int> vrcholy = new List<int>();
            
            while (vrcholy.Count != PocetVrcholov)
            {
                for (int i = 1; i <= PocetVrcholov; i++)
                {
                    if (_zastupenieVrcholov[i] == 0 && !vrcholy.Contains(i))
                    {
                        vrcholy.Add(i);
                        foreach (var hrana in SmernikyVystupne[i])
                        {
                            _zastupenieVrcholov[hrana.KonecnyVrchol]--;
                        }
                    }
                }   
            }
            
            for (int i = 0; i < PocetVrcholov; i++)
            {
                VrcholyMonotonne[i] = new Vrchol(vrcholy[i], _trvanie[vrcholy[i] - 1]);
            }

            Vrcholy = VrcholyMonotonne.OrderBy(x => x.Index).ToArray();
        }

        private void VytvorPoleSmernikovVystupnych()
        {
            SmernikyVystupne = new Dictionary<int, List<Hrana>>();
            for (int i = 1; i <= VrcholyMonotonne.Length; i++)
            {
                SmernikyVystupne[i] = new List<Hrana>();
            }
            
            foreach (var hrana in Hrany)
            {
                if (!SmernikyVystupne[hrana.ZaciatocnyVrchol].Contains(hrana))
                {
                    SmernikyVystupne[hrana.ZaciatocnyVrchol].Add(hrana);
                }
            }
        }
        
        private void VytvorPoleSmernikovVstupnych()
        {
            SmernikyVstupne = new Dictionary<int, List<Hrana>>();
            for (int i = 1; i <= VrcholyMonotonne.Length; i++)
            {
                SmernikyVstupne[i] = new List<Hrana>();
            }
            
            foreach (var hrana in Hrany)
            {
                if (!SmernikyVstupne[hrana.KonecnyVrchol].Contains(hrana))
                {
                    SmernikyVstupne[hrana.KonecnyVrchol].Add(hrana);
                }
            }
        }
    }
}
