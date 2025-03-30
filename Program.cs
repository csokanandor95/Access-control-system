﻿class Program
{
    class Tevekenyseg //1 sorban 1 tevékenység van, ezt jelképezi az osztály
    {
        //Minden egyes tevékenység külön objektum lesz
        public string Azon { get; set; } //azonosító
        public DateTime Ido { get; set; } //időpont
        public int Kod { get; set; } //kód
        public Tevekenyseg(string azon, DateTime ido, int kod) //konstruktor, ha létrehozunk egy új tevékenység objektumot, akkor annak az adatait azonnal be tudjuk állítani
        {
            Azon = azon;
            Ido = ido;
            Kod = kod;
        }
    }

    static List<Tevekenyseg> tevekenysegek = new(); //tevékenységek listája
    static void ElsoFeladat()
    {
        using (StreamReader sr = new StreamReader("bedat.txt")) //fájl beolvasása + using: a fájl bezáródik a blokk végén, így nincs memória és erőforrás szivárgás
        {
            while (!sr.EndOfStream) //while ciklussal a fájl végéig olvasunk
            {
                string[] s = sr.ReadLine().Split(" "); //szétszedi a szöveget szóközönként
                //adatokat megfelelő típusra alakítjuk
                string azon = s[0];
                DateTime ido = Convert.ToDateTime(s[1]);
                int kod = Convert.ToInt32(s[2]);

                tevekenysegek.Add(new Tevekenyseg(azon, ido, kod)); //új tevékenység objektum létrehozása és hozzáadása a listához
            }
        }
             
    }
    static void MasodikFeladat()
    {
        Console.WriteLine("2. feladat:");
        Console.WriteLine($"2. feladat: Az elso tanulo {tevekenysegek[0].Ido.Hour:02}:{tevekenysegek[0].Ido.Minute:02} -kor lepett be a fokapun. "); //2 számjegyű formátumban kell megadni az órát és a percet
        Console.WriteLine($"2. feladat: Az utolso tanulo {tevekenysegek[tevekenysegek.Count -1].Ido.Hour:02}:{tevekenysegek[tevekenysegek.Count - 1].Ido.Minute:02} -kor lepett be a fokapun. "); //.count-al az utolsó elem indexe megadható max algoritmus helyett
    }

    static void Main() //program belépési pontja, itt fogjuk meghívni a metódusokat
    {
        MasodikFeladat();
    }
}

