using System.Data.Entity;
using Layer.Business.Classes.Marka;
using Layer.Business.Classes.Yorum;
using Layer.Data;
using Layer.Entities;
using Microsoft.AspNetCore.Mvc;

namespace UI.Admin.Controllers
{
    public class YorumController : Controller
    {
        private TYorum yorumIslemleri = new();

        public IActionResult Index()
        {
            var sonuc = yorumIslemleri.OnaylanmisYorumlariListele();
            return View(sonuc.Veri);
        }

        public IActionResult OnayBekleyen()
        {
            var result = yorumIslemleri.OnayBekleyenYorumlariListele();
            return View(result.Veri as IQueryable<TblYorum>);
        }

        [HttpPost]
        public IActionResult Onayla(int id)
        {
            var result = yorumIslemleri.YorumOnayla(id);
            return Json(new { basarili = result.Basarili, mesaj = result.Mesaj });
        }
        public IActionResult Sil(int id) 
        {
            TResult result = yorumIslemleri.YorumDetayGetir(id);
            return View(result.Data as TblYorum);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Sil(int id, IFormCollection collection)
        {
            var result = yorumIslemleri.YorumSil(id);
            if (result.Basarili)
                return RedirectToAction("Index");
            else
                return View("Hata", result.Mesaj);
        }
    }
}
