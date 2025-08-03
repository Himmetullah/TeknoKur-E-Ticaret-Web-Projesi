using Layer.Business.Classes.Kategori;
using Layer.Data;
using Microsoft.AspNetCore.Mvc;

namespace UI.Customer.Controllers
{
    public class HomeController : Controller
    {
        private DbTeknoKurEntities context = new();

        public IActionResult Index()
        {
            var masustuBilgisayarKategori = context.TblKategoriler
                .FirstOrDefault(x => x.KategoriAdi == "Masaüstü Bilgisayarlar" && x.Silik != true);

            var masustuUrunler = new List<TblUrun>();
            if (masustuBilgisayarKategori != null)
            {
                masustuUrunler = context.TblUrun
                    .Where(x => x.KategoriId == masustuBilgisayarKategori.KategoriId && x.Silik != true && x.Stok > 0)
                    .OrderByDescending(x => x.EklenmeTarihi)
                    .Take(2)
                    .ToList();
            }

            ViewBag.masustuBilgisayarUrunler = masustuUrunler;

            var kulakliklar = context.TblUrun
                .Where(u => u.KategoriId == 28 && u.Silik != true && u.Stok > 0)
                .OrderByDescending(u => u.EklenmeTarihi)
                .Take(5) 
                .ToList();
            ViewBag.Kulakliklar = kulakliklar;

            var klavyeler = context.TblUrun
                .Where(u => u.KategoriId == 26 && u.Silik != true && u.Stok > 0)
                .OrderByDescending(u => u.EklenmeTarihi)
                .Take(5)
                .ToList();
            ViewBag.Klavyeler = klavyeler;

            var mouselar = context.TblUrun
                .Where(u => u.KategoriId == 27 && u.Silik != true && u.Stok > 0)
                .OrderByDescending(u => u.EklenmeTarihi)
                .Take(5)
                .ToList();
            ViewBag.Mouselar = mouselar;

            return View();
        }
        public IActionResult Hakkimizda()
        {
            return View();
        }
    }
}
