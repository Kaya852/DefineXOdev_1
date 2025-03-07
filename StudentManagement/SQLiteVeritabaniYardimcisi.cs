using System;
using System.Data.SQLite;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace OgrenciYonetim
{
    public class SQLiteVeritabaniYardimcisi
    {

        private string ConnectionString = ConfigureManager.getSQLiteConnectionString();

        public bool Ekle(object veri)
        {
            Type type = veri.GetType();
            var tabloAttr = type.GetCustomAttribute<TabloAttribute>();
            if (tabloAttr == null)
            {
                throw new Exception("Sınıfın [Tablo] özelliği yok.");
            }

            string tabloAdi = tabloAttr.Ad;

            var essizAlanlar = type.GetProperties()
                .Where(p => p.GetCustomAttribute<EssizAttribute>() != null)
                .ToArray();

            using (SQLiteConnection baglanti = new SQLiteConnection(ConnectionString))
            {
                baglanti.Open();

                foreach (var alan in essizAlanlar)
                {
                    string alanAdi = alan.Name;
                    var alanDegeri = alan.GetValue(veri);

                    string kontrolSorgusu = $"SELECT COUNT(*) FROM {tabloAdi} WHERE {alanAdi} = ?";
                    using (SQLiteCommand kontrolKomut = new SQLiteCommand(kontrolSorgusu, baglanti))
                    {
                        kontrolKomut.Parameters.AddWithValue("?", alanDegeri);
                        int sayi = Convert.ToInt32(kontrolKomut.ExecuteScalar());
                        if (sayi > 0)
                        {
                            throw new Exception($"Bu {alanAdi} zaten tabloda mevcut! Benzersiz olması gerekiyor.");
                        }
                    }
                }

                var kolonlar = type.GetProperties().Select(p => p.Name).ToArray();
                var degerler = type.GetProperties().Select(p => p.GetValue(veri)).ToArray();

                string kolonListesi = string.Join(",", kolonlar);
                string parametreListesi = string.Join(",", kolonlar.Select((k, i) => $"@p{i}"));

                string eklemeSorgusu = $"INSERT INTO {tabloAdi} ({kolonListesi}) VALUES ({parametreListesi})";

                using (SQLiteCommand eklemeKomut = new SQLiteCommand(eklemeSorgusu, baglanti))
                {
                    for (int i = 0; i < kolonlar.Length; i++)
                    {
                        eklemeKomut.Parameters.AddWithValue($"@p{i}", degerler[i] ?? DBNull.Value);
                    }

                    int sonuc = eklemeKomut.ExecuteNonQuery();
                    return sonuc > 0;
                }
            }
        }

        public List<Dictionary<string, object>> Getir<T>()
        {
            Type type = typeof(T);
            var tabloAttr = type.GetCustomAttribute<TabloAttribute>();
            if (tabloAttr == null)
            {
                throw new Exception("Sınıfın [Tablo] özelliği yok.");
            }

            string tabloAdi = tabloAttr.Ad;

            using (SQLiteConnection baglanti = new SQLiteConnection(ConnectionString))
            {
                baglanti.Open();

                string sorgu = $"SELECT * FROM {tabloAdi}";
                using (SQLiteCommand komut = new SQLiteCommand(sorgu, baglanti))
                using (SQLiteDataReader reader = komut.ExecuteReader())
                {
                    var tumKayıtlar = new List<Dictionary<string, object>>();

                    while (reader.Read())
                    {
                        var kayit = new Dictionary<string, object>();

                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            kayit[reader.GetName(i)] = reader.GetValue(i);
                        }

                        tumKayıtlar.Add(kayit);
                    }

                    return tumKayıtlar;
                }
            }
        }

        public bool Sil<T>(string essizDeger)
        {
            Type type = typeof(T);
            var tabloAttr = type.GetCustomAttribute<TabloAttribute>();
            if (tabloAttr == null)
            {
                throw new Exception("Sınıfın [Tablo] özelliği yok.");
            }

            string tabloAdi = tabloAttr.Ad;

            var essizAlan = type.GetProperties()
                .FirstOrDefault(p => p.GetCustomAttribute<EssizAttribute>() != null);

            if (essizAlan == null)
            {
                throw new Exception("Silme işlemi için benzersiz alan bulunamadı.");
            }

            string essizAlanAdi = essizAlan.Name;

            using (SQLiteConnection baglanti = new SQLiteConnection(ConnectionString))
            {
                baglanti.Open();

                string sorgu = $"DELETE FROM {tabloAdi} WHERE {essizAlanAdi} = @essizDeger";
                using (SQLiteCommand komut = new SQLiteCommand(sorgu, baglanti))
                {
                    komut.Parameters.AddWithValue("@essizDeger", essizDeger);

                    int sonuc = komut.ExecuteNonQuery();
                    return sonuc > 0;
                }
            }
        }

    }
}
