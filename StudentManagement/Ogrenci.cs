using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OgrenciYonetim
{
    [Tablo("Students")]

    public class Ogrenci
    {
        [Essiz]
        [ZorunluAlan]
        [OgrenciNumarasi]
        public string No { get; set; }
        [ZorunluAlan]
        public string Adi {  get; set; }
        [ZorunluAlan]
        public string Soyadi {  get; set; }
        [ZorunluAlan]
        public string Bolum {  get; set; }
    }
}
