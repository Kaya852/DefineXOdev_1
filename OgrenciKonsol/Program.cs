using OgrenciYonetim;

IVeriKaynagi veriKaynagi;

VeriKaynagiSec();

while (true)
{
    Console.WriteLine("Yapmak istediginiz islemi secin(yeni kayit eklemek icin Y, Kayitlari gormek icin G, kayit silmek icin S, Veri kaynagi deistirmek icin D)");
    string yapilacakIs= Console.ReadLine().Trim().ToUpper();
    Console.Clear();

    switch (yapilacakIs)
    {
        case "Y":
            YeniKayitOlustur();
            break;
        case "G":
            TumKayitlariListele();
            break;
        case "S":
            KayitSil();
            break;
        case "D":
            VeriKaynagiSec();
            break;
        default:
            Console.WriteLine("Lutfen gecerli bir secim yapin");
            break;
    }


}

void KayitSil()
{
    Console.Write("Silmek istediginiz ogrencinin numarisini girin:");
    string silenecekNumara = Console.ReadLine().Trim();
    veriKaynagi.Sil(silenecekNumara);
    Console.WriteLine("\nSilme islemi basarili!");
}
void TumKayitlariListele()
{
    var result = veriKaynagi.VeriCek();
    foreach (var kayit in result)
    {
        foreach (var item in kayit)
        {
            Console.WriteLine($"{item.Key}: {item.Value}");
        }
        Console.WriteLine("--------------------");
    }
}
void YeniKayitOlustur()
{
    Console.Write("Adınızı girin: ");
    string ad = Console.ReadLine();

    Console.Write("Soyadınızı girin: ");
    string soyad = Console.ReadLine();

    Console.Write("Bölümünüzü girin: ");
    string bolum = Console.ReadLine();

    Console.Write("Numaranizi girin: ");
    string id = Console.ReadLine();

    Ogrenci ogrenci = new Ogrenci
    {
        No = id,
        Adi = ad,
        Soyadi = soyad,
        Bolum = bolum

    };


    if (veriKaynagi.Kaydet(ogrenci, out string hataMesaji))
    {
        Console.WriteLine("\nÖğrenci Bilgileri Veritabanına Kaydedildi:");
        Console.WriteLine($"Ad: {ogrenci.Adi}");
        Console.WriteLine($"Soyad: {ogrenci.Soyadi}");
        Console.WriteLine($"Bölüm: {ogrenci.Bolum}");
        Console.Write("\nYeni kayıt yapmak ister misiniz? (E/H): ");
    }
    else
    {
        Console.WriteLine(hataMesaji);
        Console.Write("\nYeniden denemek ister misiniz? (E/H): ");
    }


    string devamMi = Console.ReadLine().Trim().ToUpper();

    if (devamMi == "E")
    {
        Console.Clear();
        YeniKayitOlustur();
    }
}

void VeriKaynagiSec()
{
    Console.Write("\nVeri Kaynagi Sec(SQLite icin S, XML icin X)");
    string veriKaynagiInput = Console.ReadLine().Trim().ToUpper();
    switch (veriKaynagiInput)
    {
        case "S":
            veriKaynagi = new SQLiteOgrenci();
            Console.WriteLine("\nSQLite veri kaynagi secildi");
            break;
        case "X":
            veriKaynagi = new XMLOgrenci();
            Console.WriteLine("\nXML veri kaynagi secildi");
            break;
        default:
            Console.WriteLine("Gecersiz giris tekrar deneyin");
            VeriKaynagiSec();
            break;

    }

}