using Microsoft.AspNetCore.Mvc;
using OgrenciYonetim; 
using System.Collections.Generic;

namespace OgrenciWeb.Controllers
{
    public class OgrenciController : Controller
    {
        private IVeriKaynagi _veriKaynagi;


        public OgrenciController(IVeriKaynagi veriKaynagi)
        {
            _veriKaynagi = veriKaynagi;
        }

        public IActionResult Index()
        {
            var result = _veriKaynagi.VeriCek() 
        .Select(dict => dict.ToDictionary(k => k.Key, v => v.Value.ToString()))
        .ToList();

            return View(result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Ogrenci ogrenci)
        {
            
                bool basarili = _veriKaynagi.Kaydet(ogrenci, out string hataMesaji);
                if (basarili)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", hataMesaji);
                }
            
            return View(ogrenci);
        }

        [HttpPost]
        public IActionResult Delete(string no)
        {
            _veriKaynagi.Sil(no);
            return RedirectToAction("Index");
        }
    }
}
