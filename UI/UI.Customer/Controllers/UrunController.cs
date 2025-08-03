using Layer.Business.Classes.Kategori;
using Layer.Business.Classes.Urun;
using Layer.Business.Classes.Yorum;
using Layer.Business.Interfaces.Kategori;
using Layer.Data;
using Microsoft.AspNetCore.Mvc;

namespace UI.Customer.Controllers
{
    public class UrunController : Controller
    {
        private DbTeknoKurEntities Context { get; set; } = new();
        private TYorum yorumIslemleri = new();

        public IActionResult Kategori(string id)
        {
            var kategori = Context.TblKategoriler.FirstOrDefault(x => x.KategoriSeo == id);
            if (kategori == null) return NotFound();

            var urunler = Context.TblUrun
                .Where(x => x.KategoriId == kategori.KategoriId && x.Silik != true)
                .ToList();

            var markaListesi = Context.TblMarka
                .Where(m => Context.TblUrun
                    .Any(u => u.KategoriId == kategori.KategoriId && u.MarkaId == m.MarkaId && u.Silik != true))
                .ToList();

            ViewBag.KategoriAdi = kategori.KategoriAdi;
            ViewBag.KategoriSeo = id;
            ViewBag.Urunler = urunler;
            ViewBag.MarkaListesi = markaListesi;

            return View();
        }

        [HttpPost]
        public IActionResult KategoriFiltre(IFormCollection form)
        {
            string kategoriSeo = form["kategoriSeo"];

            var kategori = Context.TblKategoriler.FirstOrDefault(x => x.KategoriSeo == kategoriSeo);
            if (kategori == null) return NotFound();

            var urunler = Context.TblUrun.Where(x => x.KategoriId == kategori.KategoriId && x.Silik != true).ToList();

            if (!string.IsNullOrEmpty(form["markalar"]))
            {
                var secilenMarkalar = form["markalar"].ToString().Split(',').ToList();
                urunler = urunler.Where(x => secilenMarkalar.Contains(x.TblMarka.MarkaAdi)).ToList();
            }

            if (decimal.TryParse(form["minFiyat"], out decimal minFiyat))
                urunler = urunler.Where(x => x.UrunFiyat >= minFiyat).ToList();

            if (decimal.TryParse(form["maxFiyat"], out decimal maxFiyat))
                urunler = urunler.Where(x => x.UrunFiyat <= maxFiyat).ToList();

            var markaListesi = Context.TblMarka
               .Where(m => Context.TblUrun.Any(u => u.KategoriId == kategori.KategoriId && u.MarkaId == m.MarkaId && u.Silik != true))
               .ToList();

            ViewBag.KategoriAdi = kategori.KategoriAdi;
            ViewBag.KategoriSeo = kategoriSeo;
            ViewBag.Urunler = urunler;
            ViewBag.MarkaListesi = markaListesi;

            return View("Kategori");
        }

        public IActionResult Detay(string seo)
        {
            var urun = Context.TblUrun.FirstOrDefault(x => x.UrunSeo == seo);
            if (urun == null)
                return NotFound();

            var kategori = Context.TblKategoriler.FirstOrDefault(x => x.KategoriId == urun.KategoriId);

            var digerUrunler = (from data in Context.TblUrun
                                where data.KategoriId == urun.KategoriId && data.UrunId != urun.UrunId && data.Silik != true
                                select data).Take(5).ToList();

            ViewBag.Urun = urun;
            ViewBag.Kategori = kategori;
            ViewBag.DigerUrunler = digerUrunler;

            return View();

        }
        public IActionResult Arama(string arananKelime, string kategori)
        {
            if (string.IsNullOrWhiteSpace(arananKelime))
                return RedirectToAction("Index", "Home");

            arananKelime = arananKelime.ToLower();

            var kategoriler = Context.TblKategoriler
                .FirstOrDefault(x => x.KategoriAdi.ToLower().Contains(arananKelime) && x.Silik != true);

            if (kategoriler != null)
            {
                return RedirectToAction("Kategori", new { id = kategoriler.KategoriSeo });
            }

            var urunler = Context.TblUrun
                .Where(x => x.UrunAdi.ToLower().Contains(arananKelime) && x.Silik != true)
                .ToList();

            ViewBag.AramaKelimesi = arananKelime;
            return View("Arama", urunler);
        }

        [HttpPost]
        public IActionResult YorumYap(int urunId, string yorumIcerik)
        {
            int? uyeIdInt = null;
            if (Request.Cookies["UserSession"] != null && int.TryParse(Request.Cookies["UserSession"], out int parsedUyeId))
            {
                uyeIdInt = parsedUyeId;
            }

            if (!uyeIdInt.HasValue)
            {
                return Json(new { Basarili = false, Mesaj = "Yorum yapmak için lütfen giriş yapın." });
            }

            if (yorumIcerik == null)
            {
                return Json(new { Basarili = false, Mesaj = "Yorum içeriği boş olamaz." });
            }

            var uye = Context.TblUye.FirstOrDefault(u => u.UyeId == uyeIdInt.Value);
            if (uye == null)
            {
                return Json(new { Basarili = false, Mesaj = "Kullanıcı bilgisi bulunamadı." });
            }

            var yeniYorum = new TblYorum
            {
                UyeId = uye.UyeId,
                UrunId = urunId,
                YorumIcerik = yorumIcerik,
                YorumTarihi = DateTime.Now,
                Onaylandi = false,
                Silik = false,
            };

            var sonuc = yorumIslemleri.YorumEkle(yeniYorum);

            return Json(new { Basarili = sonuc.Basarili, Mesaj = sonuc.Mesaj });
        }

        public IActionResult YorumlariGetir(int urunId)
        {
            var sonuc = yorumIslemleri.YorumlariListele(urunId);

            if (sonuc.Basarili && sonuc.Veri is IQueryable<TblYorum> yorumlar)
            {
                var yorumDetaylari = yorumlar.Select(y => new
                {
                    y.YorumIcerik,
                    y.YorumTarihi,
                }).ToList();

                return Json(new { Basarili = true, Yorumlar = yorumDetaylari });
            }

            return Json(new { Basarili = false, Mesaj = sonuc.Mesaj ?? "Yorumlar listelenirken bir hata oluştu.", Yorumlar = new List<object>() });
        }
    }
}
