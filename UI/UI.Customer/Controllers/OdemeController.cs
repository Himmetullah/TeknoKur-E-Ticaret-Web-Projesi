using Layer.Business.Classes.Siparis;
using Layer.Data;
using Microsoft.AspNetCore.Mvc;
using UI.Customer.Entities;

namespace UI.Customer.Controllers
{
    public class OdemeController : Controller
    {
        private DbTeknoKurEntities context = new();

        public IActionResult Index()
        {
            int? uyeId = null;

            if (Request.Cookies["UserSession"] != null)
            {
                if (int.TryParse(Request.Cookies["UserSession"], out int parseUyeId))
                {
                    uyeId = parseUyeId;
                }
            }

            if (uyeId == null)
            {
                return RedirectToAction("GirisYap", "Uye"); 
            }

            var sepetUrunler = context.TblSepet
                .Where(s => s.UyeId == uyeId)
                .Select(s => new
                {
                    urunAdi = s.TblUrun.UrunAdi,
                    urunFiyat = s.TblUrun.UrunFiyat,
                    adet = s.Adet,
                })
                .ToList<dynamic>();

            ViewBag.SepetUrunler = sepetUrunler;
            ViewBag.ToplamFiyat = sepetUrunler.Sum(u => (decimal)u.urunFiyat * (int)u.adet);

            return View();
        }


        [HttpPost]
        public IActionResult Index(TCrediCart cart, int adresId)
        {
            int? uyeId = null;
            if (Request.Cookies["UserSession"] != null && int.TryParse(Request.Cookies["UserSession"], out int parsedUyeId))
            {
                uyeId = parsedUyeId;
            }

            if (!uyeId.HasValue)
            {
                return RedirectToAction("GirisYap", "Uye");
            }

            TSiparis siparisIslemleri = new();

            var sonuc = siparisIslemleri.SiparisOlustur(uyeId.Value, adresId);

            if (!sonuc.Basarili)
            {
                TempData["Error"] = sonuc.Mesaj;
                return RedirectToAction("Index");
            }

            TempData["Success"] = "✔ Siparişiniz başarıyla onaylandı. Ana sayfaya yönlendiriliyorsunuz...";

            return RedirectToAction("SiparisOnay");
        }
        public IActionResult SiparisOnay()
        {
            ViewBag.Mesaj = TempData["Success"];
            return View();
        }
    }
}
