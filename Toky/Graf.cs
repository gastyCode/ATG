using System;

namespace grafy
{
    internal class Graf
    {
        public Vrchol[] Vrcholy { get; }
        public Hrana[] Hrany { get; }
        public int PocetVrcholov { get; }
        public int PocetHran { get; }
        public Dictionary<int, List<Hrana>> SmernikyVystupne { get; private set; }
        public Dictionary<int, List<Hrana>> SmernikyVstupne { get; private set; }

        public Graf(string cesta)
        {
            SmernikyVystupne = new Dictionary<int, List<Hrana>>();
            SmernikyVstupne = new Dictionary<int, List<Hrana>>();
            try
            {
                PocetVrcholov = 0;
                if (File.Exists(cesta))
                {
                    string[] riadky = File.ReadAllLines(cesta);
                    Hrany = new Hrana[riadky.Length];

                    for (int i = 0; i < riadky.Length; i++)
                    {
                        string[] rozdelenyRiadok = riadky[i].Split(" ");
                        rozdelenyRiadok = rozdelenyRiadok.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                        int[] cisla = Array.ConvertAll(rozdelenyRiadok, x => int.Parse(x));

                        Hrany[i] = new Hrana(cisla[0], cisla[1], cisla[2], cisla[3]);
                        PocetVrcholov = PocetVrcholov < cisla[0] ? cisla[0] : PocetVrcholov;
                        PocetVrcholov = PocetVrcholov < cisla[1] ? cisla[1] : PocetVrcholov;
                    }

                    PocetHran = riadky.Length;
                }

                PocetVrcholov++;
                Vrcholy = new Vrchol[PocetVrcholov];
                for (int i = 0; i < Vrcholy.Length; i++)
                {
                    Vrcholy[i] = new Vrchol(i + 1);
                }
                
                ZoradHrany();
                NastavHrany();
            }
            catch (FileNotFoundException e)
            {
                Console.Error.WriteLine(e);
            }
        }

        private void ZoradHrany()
        {
            Array.Sort(Hrany, (x, y) =>
            {
                var porovnanie = x.ZaciatocnyVrchol.CompareTo(y.ZaciatocnyVrchol);
                return porovnanie != 0 ? porovnanie : x.KonecnyVrchol.CompareTo(y.KonecnyVrchol);
            });
        }

        private void NastavHrany()
        {
            foreach (var hrana in Hrany)
            {
                if (!SmernikyVystupne.ContainsKey(hrana.ZaciatocnyVrchol)) SmernikyVystupne[hrana.ZaciatocnyVrchol] = new List<Hrana>();
                if (!SmernikyVstupne.ContainsKey(hrana.KonecnyVrchol)) SmernikyVstupne[hrana.KonecnyVrchol] = new List<Hrana>();
                
                SmernikyVystupne[hrana.ZaciatocnyVrchol].Add(hrana);
                SmernikyVstupne[hrana.KonecnyVrchol].Add(hrana);
            }
        }

        public Hrana NajdiHranu(int z, int k)
        {
            foreach (var hrana in SmernikyVystupne[z])
            {
                if (k == hrana.KonecnyVrchol)
                {
                    return hrana;
                }   
            }
            return null;
        }
    }
}
