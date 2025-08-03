using System.Security.Claims;
using Layer.Business.Classes.Sepet;
using Layer.Data;
using Microsoft.AspNetCore.Mvc;

namespace UI.Customer.Controllers
{
    public class SepetController : Controller
    {
        private TSepet sepet = new();
        private DbTeknoKurEntities context = new();

        public IActionResult Index()
        {
            int? uyeId = null;
            if (Request.Cookies["UserSession"] != null && int.TryParse(Request.Cookies["UserSession"], out int parsedUyeId))
            {
                uyeId = parsedUyeId;
            }

            var sepetListesi = new System.Collections.Generic.List<dynamic>();
            decimal toplamTutar = 0;
            bool kullaniciGirisYapmisMi = uyeId.HasValue; 

            if (!kullaniciGirisYapmisMi)
            {
                ViewBag.Mesaj = "Sepetinizdeki ürünleri görmek ve alışverişe devam etmek için lütfen giriş yapın.";
            }
            else 
            {
                var sonuc = sepet.SepetiListele(uyeId.Value);
                if (sonuc.Basarili && sonuc.Veri is IQueryable<TblSepet> veri)
                {
                    var sepetItems = veri.ToList();
                    sepetListesi = sepetItems.Join(context.TblUrun, x => x.UrunId, y => y.UrunId, (x, y) => new
                    {
                        SepetId = x.SepetId,
                        UrunId = x.UrunId,
                        UrunAdi = y.UrunAdi,
                        UrunResimUrl = y.UrunResimUrl,
                        Adet = x.Adet,
                        ToplamFiyat = y.UrunFiyat * x.Adet,
                        BirimFiyat = y.UrunFiyat,
                        Stok = y.Stok,
                        UrunSeo = y.UrunSeo,
                    }).ToList<dynamic>();

                    toplamTutar = sepetListesi.Sum(x => (decimal)x.ToplamFiyat);
                }
            }

            bool kullanicininAdresiVarmi = false;
            if (uyeId.HasValue) 
            {
                kullanicininAdresiVarmi = context.TblKisiAdres.Any(a => a.KisiId == uyeId.Value && a.Silik != true);
            }

            ViewBag.KullaniciAdresiVarmi = kullanicininAdresiVarmi;
            ViewBag.SepetUrunler = sepetListesi;
            ViewBag.ToplamFiyat = toplamTutar;
            ViewBag.KullaniciGirisYapmisMi = kullaniciGirisYapmisMi; 

            return View();
        }

        [HttpPost]
        public IActionResult SepetUrunSil(int sepetId)
        {
            var sonuc = sepet.SepetiSil(sepetId);

            int? uyeId = null;
            if (Request.Cookies["UserSession"] != null && int.TryParse(Request.Cookies["UserSession"], out int parsedUyeId))
                uyeId = parsedUyeId;

            decimal yeniSepetToplam = uyeId.HasValue ? sepet.GetSepetToplam(uyeId.Value) : 0;

            return Json(new
            {
                basarili = sonuc.Basarili,
                mesaj = sonuc.Mesaj,
                yeniSepetToplam = (double)Math.Round(yeniSepetToplam, 2)
            });
        }

        [HttpPost]
        public IActionResult SepetAdetGuncelle(int sepetId, int yeniAdet)
        {
            if (yeniAdet <= 0)
                return Json(new { basarili = false, mesaj = "Adet 0'dan büyük olmalıdır." });

            var sepetUrun = context.TblSepet.FirstOrDefault(x => x.SepetId == sepetId);
            if (sepetUrun == null)
                return Json(new { basarili = false, mesaj = "Sepet ürünü bulunamadı." });

            var urun = context.TblUrun.FirstOrDefault(x => x.UrunId == sepetUrun.UrunId && x.Silik != true);
            if (urun == null)
                return Json(new { basarili = false, mesaj = "Ürün bulunamadı." });

            if (urun.Stok < yeniAdet)
                return Json(new { basarili = false, mesaj = "Yeterli stok yok." });

            TblSepet guncellenecekSepet = new()
            {
                SepetId = sepetId,
                Adet = yeniAdet,
                ToplamFiyat = urun.UrunFiyat * yeniAdet
            };

            var sonuc = sepet.SepetiGuncelle(guncellenecekSepet);

            if (!sonuc.Basarili)
                return Json(new { basarili = false, mesaj = "Sepet güncellenirken bir hata oluştu." });

            int? uyeId = null;
            if (Request.Cookies["UserSession"] != null && int.TryParse(Request.Cookies["UserSession"], out int parsedUyeId))
                uyeId = parsedUyeId;

            decimal yeniSepetToplam = uyeId.HasValue ? sepet.GetSepetToplam(uyeId.Value) : 0;

            return Json(new
            {
                basarili = true,
                yeniToplamFiyat = (double)Math.Round(urun.UrunFiyat * yeniAdet, 2),
                yeniSepetToplam = (double)Math.Round(yeniSepetToplam, 2),
                mesaj = "Sepet güncellendi."
            });
        }

        [HttpPost]
        public IActionResult Ekle(int urunId, int adet)
        {
            int? uyeId = null;
            if (Request.Cookies["UserSession"] != null)
            {
                if (int.TryParse(Request.Cookies["UserSession"], out int parsedUyeId))
                {
                    uyeId = parsedUyeId;
                }
            }

            if (!uyeId.HasValue)
            {
                return Json(new { Basarili = false, Mesaj = "Sepete ürün eklemek için lütfen giriş yapın veya üye olun.", Yonlendir = Url.Action("GirisYap", "Uye") });
            }

            var urun = sepet.Context.TblUrun.FirstOrDefault(x => x.UrunId == urunId && x.Silik != true);

            if (urun == null)
            {
                return Json(new { Basarili = false, Mesaj = "Ürün bulunamadı." });
            }

            if (adet <= 0)
            {
                return Json(new { Basarili = false, Mesaj = "Adet 0'dan büyük olmalıdır." });
            }

            if (urun.Stok < adet)
            {
                return Json(new { Basarili = false, Mesaj = "Yeterli stok yok." });
            }

            TblSepet yeni = new()
            {
                UrunId = urunId,
                UyeId = uyeId.Value,
                Adet = adet,
                ToplamFiyat = urun.UrunFiyat * adet,
                EklenmeTarihi = DateTime.Now,
            };

            var sonuc = sepet.SepeteEkle(yeni);

            if (!sonuc.Basarili)
            {
                return Json(new { Basarili = false, Mesaj = sonuc.Mesaj });
            }

            var guncelSepetAdediSonuc = sepet.SepetiListele(uyeId.Value);
            int sepetAdedi = 0;

            if (guncelSepetAdediSonuc.Basarili && guncelSepetAdediSonuc.Veri != null)
            {
                sepetAdedi = ((IQueryable<TblSepet>)guncelSepetAdediSonuc.Veri).Sum(x => x.Adet);
            }

            var urunBilgisi = new
            {
                Ad = urun.UrunAdi,
                Fiyat = urun.UrunFiyat.ToString("C2"),
                Resim = urun.UrunResimUrl
            };

            return Json(new
            {
                basarili = true,
                mesaj = "Ürün başarıyla sepete eklendi!",
                Urun = urunBilgisi,
                sepetAdet = sepetAdedi
            });
        }

        [HttpGet]
        public IActionResult GetSepetItemCount()
        {
            int? uyeId = null;
            if (Request.Cookies["UserSession"] != null)
            {
                if (int.TryParse(Request.Cookies["UserSession"], out int parsedUyeId))
                {
                    uyeId = parsedUyeId;
                }
            }

            if (!uyeId.HasValue)
            {
                return Json(new { Basarili = true, SepetAdet = 0 });
            }

            try
            {
                var sepetListeleSonuc = sepet.SepetiListele(uyeId.Value);
                int sepetAdet = 0;

                if (sepetListeleSonuc.Basarili && sepetListeleSonuc.Veri != null)
                {
                    var veri = sepetListeleSonuc.Veri as IQueryable<TblSepet>;
                    if (veri != null)
                        sepetAdet = veri.Sum(x => x.Adet);
                }

                return Json(new { Basarili = true, SepetAdet = sepetAdet });
            }
            catch (Exception ex)
            {
                return Json(new { Basarili = false, SepetAdet = 0, Mesaj = "Server error: " + ex.Message });
            }

        }
        public IActionResult GetUrunDetay(int urunId)
        {
            var urun = context.TblUrun
                              .Where(x => x.UrunId == urunId && x.Silik != true)
                              .Select(x => new
                              {
                                  urunAdi = x.UrunAdi,
                                  urunFiyat = x.UrunFiyat,
                                  urunResimUrl = x.UrunResimUrl,
                                  stok = x.Stok
                              })
                              .FirstOrDefault();

            if (urun == null)
                return NotFound();
            return Json(urun);
        }
        public IActionResult GetCartTotals()
        {
            int? uyeId = null;
            if (Request.Cookies["UserSession"] != null && int.TryParse(Request.Cookies["UserSession"], out int parsedUyeId))
            {
                uyeId = parsedUyeId;
            }

            decimal yeniSepetToplam = 0;
            if (uyeId.HasValue)
            {
                var tumSepetUrunleri = context.TblSepet
                    .Where(x => x.UyeId == uyeId.Value)
                    .ToList();

                yeniSepetToplam = tumSepetUrunleri.Sum(x => x.ToplamFiyat);
            }

            return Json(new
            {
                basarili = true,
                yeniSepetToplam = (double)Math.Round(yeniSepetToplam, 2)
            });
        }
        

    }
}
