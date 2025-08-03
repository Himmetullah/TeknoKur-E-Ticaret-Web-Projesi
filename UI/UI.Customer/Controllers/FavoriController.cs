using Layer.Business.Classes.Favori;
using Layer.Data;
using Microsoft.AspNetCore.Mvc;

namespace UI.Customer.Controllers
{
    public class FavoriController : Controller
    {
        private DbTeknoKurEntities Context { get; set; } = new();
        private TFavori favori = new();

        public IActionResult Favoriler()
        {
            int? uyeId = null;

            if (Request.Cookies["UserSession"] != null)
            {
                int parseUyeId;
                if (int.TryParse(Request.Cookies["UserSession"], out parseUyeId))
                {
                    uyeId = parseUyeId;
                }
            }

            List<object> favoriUrunDetaylar = new List<object>();

            if (uyeId.HasValue)
            {
                var favoriResult = favori.FavoriListele(uyeId.Value);

                if (favoriResult.Basarili && favoriResult.Veri != null)
                {
                    List<TblFavori> kullaniciFavorileri = ((IQueryable<TblFavori>)favoriResult.Veri).ToList();

                    favoriUrunDetaylar = (from f in kullaniciFavorileri
                                          join u in Context.TblUrun on f.UrunId equals u.UrunId
                                          where u.Silik != true
                                          select new
                                          {
                                              FavoriId = f.FavoriId,
                                              UrunId = u.UrunId,
                                              UrunAdi = u.UrunAdi,
                                              Fiyat = u.UrunFiyat,
                                              StokDurumu = u.Stok > 0 ? "Stokta Var" : "Stokta Yok",
                                              ResimUrl = u.UrunResimUrl
                                          }).Cast<object>().ToList();
                }
            }

            ViewBag.FavoriListesi = favoriUrunDetaylar;
            ViewBag.IsUserLoggedIn = uyeId.HasValue;

            if (!uyeId.HasValue)
            {
                ViewBag.Mesaj = "Favori listenizi görmek için lütfen giriş yapın.";
            }
            else if (!favoriUrunDetaylar.Any())
            {
                ViewBag.Mesaj = "Favorilerinize eklediğiniz ürün bulunmamaktadır.";
            }
            return View();

        }

        [HttpPost]
        public IActionResult Ekle(int urunId)
        {
            int uyeId = 0;
            if (Request.Cookies["UserSession"] != null)
            {
                if (!int.TryParse(Request.Cookies["UserSession"], out uyeId))
                {
                    return Json(new { Basarili = false, Mesaj = "Kullanıcı oturumu geçersiz.", Misafir = false });
                }
            }
            else
            {
                return Json(new { Basarili = false, Misafir = true, Mesaj = "Favorilere eklemek için lütfen giriş yapın." });
            }

            TblFavori yeni = new()
            {
                UyeId = uyeId,
                UrunId = urunId,
            };

            var sonuc = favori.FavoriEkle(yeni);

            if (sonuc.Basarili)
                return Json(new { Basarili = true, Mesaj = sonuc.Mesaj }); // Başarılı mesajını döndür
            else
                return Json(new { Basarili = false, Mesaj = sonuc.Mesaj }); // Hata mesajını döndür
        }

        [HttpPost]
        public IActionResult FavoriSil(int favoriId)
        {
            int uyeId = 0;

            if (Request.Cookies["UserSession"] != null)
                uyeId = int.Parse(Request.Cookies["UserSession"]);
            else
                return Json(new { Basarili = false });

            var favoriKayit = (from data in Context.TblFavori
                               where data.UyeId == uyeId && data.FavoriId == favoriId && data.Silik != true
                               select data).FirstOrDefault();

            if (favoriKayit != null)
            {
                var sonuc = favori.FavoriSil(favoriKayit.FavoriId);

                if (sonuc.Basarili)
                    return Json(new { Basarili = true, Mesaj = "Favori başarıyla kaldırıldı." });
                else
                    return Json(new { Basarili = false, Mesaj = sonuc.Mesaj });
            }
            return Json(new { Basarili = false, Mesaj = "Favori bulunamadı veya size ait değil." });
        }
        public IActionResult Say()
        {
            int uyeId = 0;

            if (Request.Cookies["UserSession"] != null)
                uyeId = int.Parse(Request.Cookies["UserSession"]);
            else
                return Json(new { Say = 0 });

            int sayi = Context.TblFavori.Count(x => x.UyeId == uyeId && x.Silik != true);
            return Json(new { Say = sayi });
        }
        public IActionResult UrunIdler()
        {
            int uyeId = 0;

            if (Request.Cookies["UserSession"] != null)
                uyeId = int.Parse(Request.Cookies["UserSession"]);
            else
                return Json(new { UrunIdler = new int[0] });

            var favoriUrunIdler = Context.TblFavori
                .Where(x => x.UyeId == uyeId && x.Silik != true)
                .Select(x => x.UrunId)
                .ToList();

            return Json(new { UrunIdler = favoriUrunIdler });
        }
        public IActionResult GetFavoriIdByUrunId(int urunId)
        {
            int uyeId = 0;
            if (Request.Cookies["UserSession"] != null)
            {
                if (!int.TryParse(Request.Cookies["UserSession"], out uyeId))
                {
                    return Json(new { basarili = false, mesaj = "Kullanıcı oturumu geçersiz." });
                }
            }
            else
            {
                return Json(new { basarili = false, mesaj = "Kullanıcı giriş yapmamış." });
            }

            var favoriKayit = Context.TblFavori
                                     .FirstOrDefault(f => f.UyeId == uyeId && f.UrunId == urunId && f.Silik != true);

            if (favoriKayit != null)
            {
                return Json(new { basarili = true, favoriId = favoriKayit.FavoriId });
            }
            else
            {
                return Json(new { basarili = false, mesaj = "Favori kaydı bulunamadı." });
            }

        }
    }
}
