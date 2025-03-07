using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OgrenciYonetim
{
    public interface IVeriKaynagi
    {
        public bool Kaydet(object dogrulanacakEntity, out string hataMesaji);
        public List<Dictionary<string, object>> VeriCek();
        public bool Sil(string ogrenciNo);

    }
}
