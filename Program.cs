using System.Linq;

//Csóka Nándor

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
        try //fájlmegnyitást védi
        {
            using (StreamReader sr = new StreamReader("bedat.txt")) //fájl beolvasása + using: a fájl bezáródik a blokk végén, így nincs memória és erőforrás szivárgás
            {
                while (!sr.EndOfStream) //while ciklussal a fájl végéig olvasunk
                {
                    string[] s = sr.ReadLine().Split(" "); //szétszedi a szöveget szóközönként

                    try //sorok beolvasását figyeli
                    {

                        string azon = s[0]; //adatokat megfelelő típusra alakítjuk
                        DateTime ido = Convert.ToDateTime(s[1]); //aktuális dátumra konvertálja, így a hatos feladatban meg kell adni az évet, hónapot, napot is, nem lehet 1, 1, 1
                        int kod = Convert.ToInt32(s[2]);

                        tevekenysegek.Add(new Tevekenyseg(azon, ido, kod)); //új tevékenység objektum létrehozása és hozzáadása a listához
                    } 

                    catch (FormatException) //ha nem megfelelő formátumú adat van
                    {
                        Console.WriteLine("1. feladat hiba: Hibás adatformátum a fájlban. Az adott sor kihagyásra került.");
                    }
                }
            }
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("1. feladat hiba: A 'bedat.txt' fájl nem található!");
            return; //kilépünk a metódusból
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ismeretlen hiba történt: {ex.Message}");
            return; //kilépünk a metódusból
        }


    Console.WriteLine("1. feladat:\nAz adatok beolvasasa megtortent.");

    }
    static void MasodikFeladat()
    {
        Console.WriteLine("2. feladat:"); // $ string interpoláció: idézőjelek közötti szövegben bele lehet ágyazni változókat vagy kifejezéseket anélkül, hogy külön kellene őket összefűzni
        Console.WriteLine($"Az elso tanulo {tevekenysegek[0].Ido.Hour:00}:{tevekenysegek[0].Ido.Minute:00} -kor lepett be a fokapun. "); //2 számjegyű formátumban kell megadni az órát és a percet
        Console.WriteLine($"Az utolso tanulo {tevekenysegek[tevekenysegek.Count - 1].Ido.Hour:00}:{tevekenysegek[tevekenysegek.Count - 1].Ido.Minute:00} -kor lepett ki a fokapun. "); //.count-al az utolsó elem indexe megadható max algoritmus helyett
    }

    static void HarmadikFeladat()
    {
        DateTime alap = DateTime.Now; // hogy ne kelljen kézzel változtatni a dátumot
        Console.WriteLine("3. feladat:");
        using (StreamWriter sw = new StreamWriter("kesok.txt")) //fájl írása a későkről
        {
            for (int i = 0; i < tevekenysegek.Count; i++) //végigmegyünk a tevékenységeken
            {
                DateTime kezdido = new(alap.Year, alap.Month, alap.Day, 7, 50, 0); //kezdőidő 7:50
                DateTime vegido = new(alap.Year, alap.Month, alap.Day, 8, 15, 0); //végeidő 8:15
                if (tevekenysegek[i].Kod == 1 && tevekenysegek[i].Ido > kezdido && tevekenysegek[i].Ido <= vegido) //ha a kód 1, azaz belépés történt, és az idő a megadott intervallumban van = késés történt
                {
                    sw.WriteLine($"{tevekenysegek[i].Ido.Hour:00}:{tevekenysegek[i].Ido.Minute:00} {tevekenysegek[i].Azon}"); //kiírjuk a késők azonosítóját
                }

            }
        }
        Console.WriteLine("A fajlba kiiras megtortent.");
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
        Console.WriteLine("6. feladat:");
        Console.WriteLine("Az erintett tanulok: ");
        List<string> tanulok = new(); //csinálunk egy listát, amiben az összes tanuló azonosítója lesz

        foreach (var t in tevekenysegek) //végigmegyünk és ha még nincs benne a listában, akkor hozzáadjuk
        {
            if (!tanulok.Contains(t.Azon))
            {
                tanulok.Add(t.Azon);
            }
        }

        foreach (var tanulo in tanulok)
        {
            DateTime alap = DateTime.Now; //hogy ne kelljen kézzel állítgatni
            bool bentVan = false; //alapból nincs bent
            DateTime kezdIdo = new(alap.Year, alap.Month, alap.Day, 10, 50, 0); //kezdőidő 10:50
            DateTime vegIdo = new(alap.Year, alap.Month, alap.Day, 11, 0, 0); //végeidő 11:00
            foreach (var t in tevekenysegek)
            {
                if (t.Azon == tanulo)
                {
                    if (t.Kod == 1 && bentVan && t.Ido >= kezdIdo && t.Ido <= vegIdo) //ha a kód 1 és bent van a megadott intervallumban
                    {
                        Console.Write($"{t.Azon} "); //kiírjuk az azonosítót
                    }
                    else if (t.Kod == 1) bentVan = true; //ha a kód 1, akkor bent van
                    else if (t.Kod == 2) bentVan = false; //ha a kód 2, akkor kint van
                }
                
            }
        }
        Console.WriteLine();
    }

    static void HetedikFeladat()
    {
        Console.WriteLine("7. feladat: ");
        Console.Write("Kerem egy tanulo azonositojat: ");

        string bekertAzon = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(bekertAzon)) // Ellenőrizzük, hogy a beírt azonosító nem üres vagy csak szóközökből áll-e
        {
            Console.WriteLine("7. feladat hiba: Az azonosito nem lehet ures!");
            return; //ha igen, kilépünk
        }
        
        DateTime elsoBelepes = new(1, 1, 1); //első belépés időpontja
        bool vanIlyenTanulo = false; //tanulo létezik-e az adatok között

        try
        {
            foreach (var t in tevekenysegek)
            {
                if (t.Azon == bekertAzon) // Ha az azonosító megegyezik a bekérttel
                {
                    elsoBelepes = t.Ido; // Elmentjük az első előfordulás időpontját
                    vanIlyenTanulo = true; // Megjegyezzük, hogy létezik ez a tanuló
                    break; // Kilépünk a ciklusból, mert megtaláltuk a tanulót
                }
            }

            if (!vanIlyenTanulo)
            {
                Console.WriteLine("Nincs ilyen tanulo aznap."); // Ha nincs ilyen tanuló az adatokban, kiírjuk és kilépünk
                return;
            }
             
            DateTime utolsoKilepes = new(1, 1, 1); // Visszafelé haladunk a listában (mert az utolsó kilépést keressük)
            for (int i = tevekenysegek.Count - 1; i >= 0; i--)
            {
                if (tevekenysegek[i].Azon == bekertAzon) 
                {
                    utolsoKilepes = tevekenysegek[i].Ido; // Elmentjük az utolsó kilépési időpontot
                    break;
                }
            }

            TimeSpan elteltIdo = utolsoKilepes - elsoBelepes; // Kiszámítjuk az eltelt időt az első belépés és az utolsó kilépés között
            Console.WriteLine($"A tanulo belepese es kilepese kozott {elteltIdo.Hours} ora es {elteltIdo.Minutes} perc telt el.");
        }
        catch (Exception ex) // Ha bármi hiba van, azt jelzi
        {
            Console.WriteLine($"Hiba tortent a feldolgozas soran: {ex.Message}");
        }
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

       

