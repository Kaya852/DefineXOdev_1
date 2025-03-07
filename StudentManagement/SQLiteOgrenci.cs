using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OgrenciYonetim
{
    public class SQLiteOgrenci : IVeriKaynagi
    {
        SQLiteVeritabaniYardimcisi helper = new SQLiteVeritabaniYardimcisi();
        public bool Kaydet(object dogrulanacakEntity, out string hataMesaji)
        {
            hataMesaji = "";
            if (!ZorunluAlanAttribute.Dogrula(dogrulanacakEntity))
            {
                hataMesaji = "Tum alanlar doldurulmali";
                return false;
            }

            else if (!OgrenciNumarasiAttribute.Dogrula(dogrulanacakEntity))
            {
                hataMesaji = "Ogrenci Numarasi Yalnis formatta(8 haneli ve sadece rakamlardan olusmalidir)";
                return false;
            }
            try
            {
                helper.Ekle(dogrulanacakEntity);
            }
            catch (Exception ex) { 
            hataMesaji = ex.Message;
            return false;
            }
            return true;

        }

       public List<Dictionary<string, object>> VeriCek()
        {
            var result = helper.Getir<Ogrenci>();
            return result;
        }

        public bool Sil(string studentNumber)
        {
            return helper.Sil<Ogrenci>(studentNumber);
        }






    }
}
