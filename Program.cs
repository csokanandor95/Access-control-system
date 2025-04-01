using System.Linq;

class Program
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
                DateTime ido = Convert.ToDateTime(s[1]); //aktuális dátumra konvertálja, így a hatos feladatban meg kell adni az évet, hónapot, napot is, nem lehet 1, 1, 1
                int kod = Convert.ToInt32(s[2]);

                tevekenysegek.Add(new Tevekenyseg(azon, ido, kod)); //új tevékenység objektum létrehozása és hozzáadása a listához
            }
        }

    }
    static void MasodikFeladat()
    {
        Console.WriteLine("2. feladat:"); // $ string interpoláció: idézőjelek közötti szövegben bele lehet ágyazni változókat vagy kifejezéseket anélkül, hogy külön kellene őket összefűzni
        Console.WriteLine($"2. feladat: Az elso tanulo {tevekenysegek[0].Ido.Hour:00}:{tevekenysegek[0].Ido.Minute:00} -kor lepett be a fokapun. "); //2 számjegyű formátumban kell megadni az órát és a percet
        Console.WriteLine($"2. feladat: Az utolso tanulo {tevekenysegek[tevekenysegek.Count - 1].Ido.Hour:00}:{tevekenysegek[tevekenysegek.Count - 1].Ido.Minute:00} -kor lepett ki a fokapun. "); //.count-al az utolsó elem indexe megadható max algoritmus helyett
    }

    static void HarmadikFeladat()
    {
        Console.WriteLine("3. feladat:");
        using (StreamWriter sw = new StreamWriter("kesok.txt")) //fájl írása a későkről
        {
            for (int i = 0; i < tevekenysegek.Count; i++) //végigmegyünk a tevékenységeken
            {
                DateTime kezdido = new(1, 1, 1, 7, 50, 0); //kezdőidő 7:50
                DateTime vegido = new(1, 1, 1, 8, 15, 0); //végeidő 8:15
                if (tevekenysegek[i].Kod == 1 && tevekenysegek[i].Ido > kezdido && tevekenysegek[i].Ido <= vegido) //ha a kód 1, azaz belépés történt, és az idő a megadott intervallumban van = késés történt
                {
                    sw.WriteLine($"{tevekenysegek[i].Ido.Hour:00}:{tevekenysegek[i].Ido.Minute:00} {tevekenysegek[i].Azon}"); //kiírjuk a késők azonosítóját
                }

            }
        }
        Console.WriteLine("A fajlba kiiras megtortent");
    }

    static int menzan_ebedlo_db = 0; //osztályszintű változó, hogy a 4. és 5. feladatban is elérhető legyen
    static void NegyedikFeladat()
    {
        Console.WriteLine("4. feladat:");
        menzan_ebedlo_db = 0; //menzán ebédelt tanulók száma
        for (int i = 0; i < tevekenysegek.Count; i++) //végigmegyünk a tevékenységeken (lehetne foreach is, mert csak simán végigmegyünk a listán)
        {
            if (tevekenysegek[i].Kod == 3) //ha a kód 3, azaz ebédelt a tanuló
            {
                menzan_ebedlo_db++; //növeljük a menzán ebédelt tanulók számát
            }
        }
        Console.WriteLine(menzan_ebedlo_db + " diak ebedelt a menzan aznap."); //kiírjuk a menzán ebédelt tanulók számát
    }

    static void OtodikFeladat()
    {
        Console.WriteLine("5. feladat:");
        int kolcsonzokDb = 0;
        List<string> kolcsonzoDiakok = new(); //kolcsonzoknek készítűnk egy listát
        for (int i = 0; i < tevekenysegek.Count; i++)
        {
            if (tevekenysegek[i].Kod == 4 && !kolcsonzoDiakok.Contains(tevekenysegek[i].Azon)) //kizárjuk a duplikációkat
            {
                kolcsonzokDb++;
                kolcsonzoDiakok.Add(tevekenysegek[i].Azon);
            }
        }
        Console.WriteLine(kolcsonzokDb + " diák kolcsonzott aznap a konyvtarban.");

        if (kolcsonzokDb > menzan_ebedlo_db)
        {
            Console.WriteLine("Tobben voltak, mint a menzan.");
        }
        else
        {
            Console.WriteLine("Nem voltak tobben, mint a menzan.");
        }

        
    }
    static void HatodikFeladat()
    {
        Console.WriteLine("6. feladat: ");
        Console.WriteLine("Az erintett tanulok:");

        DateTime kilepesKezdete = new(2025, 04, 01, 10, 45, 0); // 10:45-kor kezdtek kisurranni
        DateTime kapuZaras = new(2025, 04, 01, 10, 50, 0); // 10:50-kor zárták a hátsó kaput

        HashSet<string> hatsokapunKimentTanulok = new(); // Halmaz a hátsó kapun kimentek azonosítóival (itt jobb mint a lista, mert nem a sorrend szamit, hanem az elemek egyedisége)

        // 1. Lépés: Azonosítjuk, kik mentek ki a hátsó kapun 10:45 és 10:50 között
        foreach (var tevekenyseg in tevekenysegek) //foreach itt egyszerűbb, mert csak végigmegyünk a listán
        {
            if (tevekenyseg.Kod == 2 && // 2-es kód = kilépés
                tevekenyseg.Ido >= kilepesKezdete && tevekenyseg.Ido < kapuZaras)
            {
                hatsokapunKimentTanulok.Add(tevekenyseg.Azon);
            }
        }

        DateTime visszajovetelKezdete = new(2025, 04, 01, 10, 50, 0); // 10:50-től kell vizsgálni a visszatérést
        DateTime szunetVege = new(2025, 04, 01, 11, 00, 0); // 11:00-kor véget ért a szünet

        // 2. Lépés: Megnézzük, hogy ezek a tanulók a főkapun jöttek-e vissza 10:50 és 11:00 között
        foreach (var tevekenyseg in tevekenysegek)
        {
            if (tevekenyseg.Kod == 1 && // 1-es kód = belépés
                hatsokapunKimentTanulok.Contains(tevekenyseg.Azon) && // Csak azokat nézzük, akik a hátsó kapun mentek ki
                tevekenyseg.Ido >= visszajovetelKezdete && tevekenyseg.Ido <= szunetVege)
            {
                Console.Write(tevekenyseg.Azon + " "); // Egy sorba, szóközzel elválasztva írjuk ki
            }
        }
        Console.WriteLine();
    }
    static void HetedikFeladat()
    {
        Console.WriteLine("7. feladat: ");
        Console.WriteLine("Kerem egy tanulo azonositojat=");
        string bekertAzon = Console.ReadLine(); //bekérjük egy tanuló azonosítóját
        DateTime elsoBelepes = new(1, 1, 1);

        for (int i = 0; i < tevekenysegek.Count; i++) //végigmegyünk a tevékenységeken
        {
            if (tevekenysegek[i].Azon == bekertAzon) //ha egy tanuló azonosítója megegyezik a bekérttel, akkor a hozzá tartozó időt elmentjük
            {
                elsoBelepes = tevekenysegek[i].Ido;
                break; //ha megvan az első belépés, akkor kilépünk a ciklusból (nincs értelme tovább menni)
            }
            else if (i == tevekenysegek.Count - 1) //ha végigmegyünk a tevékenységeken és nem találjuk meg a bekért azonosítót, akkor kiírjuk, hogy nincs ilyen tanuló
            {
                Console.WriteLine("Nincs ilyen tanulo aznap.");
                return; //kilépünk a metódusból
            }
        }

        DateTime utolsoKilepes = new(1, 1, 1);

        for (int i = tevekenysegek.Count - 1; i >= 0; i--) //végigmegyünk a tevékenységeken visszafelé
        {
            if (tevekenysegek[i].Azon == bekertAzon) //ha egy tanuló azonosítója megegyezik a bekérttel, akkor a hozzá tartozó időt elmentjük
            {
                utolsoKilepes = tevekenysegek[i].Ido;
                break; //ha megvan az utolsó kilépés, akkor kilépünk a ciklusból (nincs értelme tovább menni)
            }
        }

        TimeSpan elteltIdo = utolsoKilepes - elsoBelepes; //az eltelt idő a két időpont különbsége
        Console.WriteLine($"A tanulo belepese es kilepese kozott  { elteltIdo.Hours } ora es { elteltIdo.Minutes } perc telt el."  ); //kiírjuk az eltelt időt

    }

    static void Main() //program belépési pontja, itt fogjuk meghívni a metódusokat
    {
        ElsoFeladat();
        MasodikFeladat();
        HarmadikFeladat();
        NegyedikFeladat();
        OtodikFeladat();
        HatodikFeladat();
        HetedikFeladat();
    }
}

