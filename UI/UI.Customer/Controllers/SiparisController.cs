using Layer.Business.Classes.Sepet;
using Layer.Business.Classes.Siparis;
using Layer.Business.Classes.Uye;
using Layer.Business.Interfaces.Sepet;
using Layer.Data;
using Microsoft.AspNetCore.Mvc;

namespace UI.Customer.Controllers
{
    public class SiparisController : Controller
    {
        private DbTeknoKurEntities Context = new();
        private TSepet sepet = new();
        private TKisiAdres kisiAdresIslemleri = new();
        private TSiparis siparisIslemleri = new();

        public IActionResult AdresSec()
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

            var kullaniciAdresleri = Context.TblKisiAdres
                                                .Where(a => a.KisiId == uyeId.Value && a.Silik != true)
                                                .ToList();

            var sepetListesi = new System.Collections.Generic.List<dynamic>();
            decimal toplamTutar = 0;

            var sonuc = sepet.SepetiListele(uyeId.Value);
            if (sonuc.Basarili && sonuc.Veri is IQueryable<TblSepet> veri)
            {
                var sepetItems = veri.ToList();
                sepetListesi = sepetItems.Join(Context.TblUrun, x => x.UrunId, y => y.UrunId, (x, y) => new
                {
                    SepetId = x.SepetId,
                    UrunId = x.UrunId,
                    UrunAdi = y.UrunAdi,
                    UrunResimUrl = y.UrunResimUrl,
                    Adet = x.Adet,
                    ToplamFiyat = x.ToplamFiyat,
                    BirimFiyat = y.UrunFiyat,
                    Stok = y.Stok,
                    UrunSeo = y.UrunSeo,
                }).ToList<dynamic>();

                toplamTutar = sepetListesi.Sum(x => (decimal)x.ToplamFiyat);
            }

            ViewBag.KullaniciAdresleri = kullaniciAdresleri;
            ViewBag.SepetUrunler = sepetListesi;
            ViewBag.ToplamFiyat = toplamTutar;

            return View();
        }
        [HttpPost]
        public IActionResult YeniAdresEkle(TblKisiAdres yeniAdres)
        {
            int? uyeId = null;
            if (Request.Cookies["UserSession"] != null && int.TryParse(Request.Cookies["UserSession"], out int parsedUyeId))
            {
                uyeId = parsedUyeId;
            }

            if (!uyeId.HasValue)
            {
                return Json(new { success = false, message = "Lütfen giriş yapın." });
            }

            if (ModelState.IsValid)
            {
                yeniAdres.KisiId = uyeId.Value;
                yeniAdres.EklenmeTarihi = DateTime.Now;
                yeniAdres.EkleyenUyeId = uyeId.Value;
                yeniAdres.Silik = false;

                Context.TblKisiAdres.Add(yeniAdres);
                Context.SaveChanges();

                return Json(new { success = true, message = "Adres başarıyla eklendi.", adresId = yeniAdres.AdresId });
            }
            return Json(new { success = false, message = "Adres eklenirken bir hata oluştu. Lütfen tüm alanları doldurunuz." });
        }
        [HttpPost]
        public IActionResult AdresSil(int adresId)
        {
            int? uyeId = null;
            if (Request.Cookies["UserSession"] != null && int.TryParse(Request.Cookies["UserSession"], out int parsedUyeId))
            {
                uyeId = parsedUyeId;
            }

            if (!uyeId.HasValue)
            {
                return Json(new { success = false, message = "Bu işlemi yapmak için lütfen giriş yapın." });
            }

            var adres = Context.TblKisiAdres
                               .FirstOrDefault(x => x.AdresId == adresId && x.KisiId == uyeId.Value); 

            if (adres == null)
            {
                return Json(new { success = false, message = "Adres bulunamadı veya bu adresi silme yetkiniz yok." });
            }

            adres.Silik = true; 
            Context.SaveChanges(); 

            return Json(new { success = true, message = "Adres başarıyla silindi." });
        }
    }
}
