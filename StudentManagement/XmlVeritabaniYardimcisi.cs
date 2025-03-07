using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace OgrenciYonetim
{
    public class XmlVeritabaniYardimcisi
    {
        private string XmlDosyaYolu = ConfigureManager.getXMLPath();

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

            XDocument doc = File.Exists(XmlDosyaYolu) ? XDocument.Load(XmlDosyaYolu) : new XDocument(new XElement("DefineXOdev1Db"));
            XElement root = doc.Root;
            XElement tablo = root.Element(tabloAdi) ?? new XElement(tabloAdi);

            foreach (var alan in essizAlanlar)
            {
                string alanAdi = alan.Name;
                var alanDegeri = alan.GetValue(veri)?.ToString();

                if (tablo.Elements("Kayit").Any(x => (string)x.Element(alanAdi) == alanDegeri))
                {
                    throw new Exception($"Bu {alanAdi} zaten tabloda mevcut! Benzersiz olması gerekiyor.");
                }
            }

            XElement yeniKayit = new XElement("Kayit");
            foreach (var prop in type.GetProperties())
            {
                yeniKayit.Add(new XElement(prop.Name, prop.GetValue(veri) ?? ""));
            }

            tablo.Add(yeniKayit);
            if (root.Element(tabloAdi) == null)
            {
                root.Add(tablo);
            }

            doc.Save(XmlDosyaYolu);
            return true;
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
            if (!File.Exists(XmlDosyaYolu)) return new List<Dictionary<string, object>>();

            XDocument doc = XDocument.Load(XmlDosyaYolu);
            XElement root = doc.Root;
            XElement tablo = root.Element(tabloAdi);
            if (tablo == null) return new List<Dictionary<string, object>>();

            return tablo.Elements("Kayit").Select(kayit =>
                kayit.Elements().ToDictionary(e => e.Name.LocalName, e => (object)e.Value)
            ).ToList();
        }

        public bool Sil<T>(string essizDeger)
        {
            if (!File.Exists(XmlDosyaYolu)) return false;

            Type type = typeof(T);
            var tabloAttr = type.GetCustomAttribute<TabloAttribute>();
            if (tabloAttr == null) return false;

            string tabloAdi = tabloAttr.Ad;

            XDocument doc;
            using (FileStream fs = new FileStream(XmlDosyaYolu, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
            {
                doc = XDocument.Load(fs);
            }

            XElement root = doc.Root;
            XElement tablo = root.Element(tabloAdi);
            if (tablo == null) return false;

            // Benzersiz (Essiz) olarak işaretlenen alanı bul
            var essizAlan = type.GetProperties()
                .FirstOrDefault(p => p.GetCustomAttribute<EssizAttribute>() != null);

            if (essizAlan == null) return false; // Eğer benzersiz alan yoksa işlem yapılmaz

            string essizAlanAdi = essizAlan.Name;
            var kayit = tablo.Elements("Kayit")
                .FirstOrDefault(x => (string)x.Element(essizAlanAdi) == essizDeger);

            if (kayit != null)
            {
                kayit.Remove();
                doc.Save(XmlDosyaYolu);
                return true;
            }
            return false;
        }

    }
}
