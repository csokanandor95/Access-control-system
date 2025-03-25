class Program
{
    class Tevekenyseg //1 sorban 1 tevékenység van, ezt jelképezi az osztály
    {
        public string Azon { get; set; } //azonosító
        public DateTime Ido { get; set; } //időpont
        public int Kod { get; set; } //kód
        public Tevekenyseg(string azon, DateTime ido, int kod) //konstruktor
        {
            Azon = azon;
            Ido = ido;
            Kod = kod;
        }
    }

    static void ElsoFeladat()
    {
        using (StreamReader sr = new StreamReader("bedat.txt"))
        {
            while (!sr.EndOfStream)
            {
                string[] s = sr.ReadLine().Split(" ");
                
            }
        }
    static void Main()
    {

    }

}

