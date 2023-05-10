using System;

namespace grafy
{
    internal class Graf
    {
        public Vrchol[] Vrcholy { get; }
        public Hrana[] Hrany { get; }
        public int PocetVrcholov { get; }
        public int PocetHran { get; }
        public Dictionary<int, List<Hrana>> Smerniky { get; }

        public Graf(string cesta)
        {
            Smerniky = new Dictionary<int, List<Hrana>>();
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

                        Hrany[i] = new Hrana(cisla[0], cisla[1], cisla[2]);
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
                
                VytvorPoleSmernikov();
            }
            catch (FileNotFoundException e)
            {
                Console.Error.WriteLine(e);
            }
        }

        private void ZoradHrany()
        {
            Array.Sort(Hrany, (x, y) =>  x.Cena.CompareTo(y.Cena));
        }

        private void VytvorPoleSmernikov()
        {
            ZoradHrany();
            
            foreach (var hrana in Hrany)
            {
                if (!Smerniky.ContainsKey(hrana.ZaciatocnyVrchol))
                {
                    Smerniky.Add(hrana.ZaciatocnyVrchol, new List<Hrana>());
                }
                var smernik = Smerniky[hrana.ZaciatocnyVrchol];
                if (!smernik.Contains(hrana))
                {
                    smernik.Add(hrana);
                }
            }
        }
    }
}
