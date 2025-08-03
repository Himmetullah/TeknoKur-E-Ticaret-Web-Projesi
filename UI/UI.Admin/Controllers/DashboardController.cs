using Layer.Data;
using Microsoft.AspNetCore.Mvc;

namespace UI.Admin.Controllers
{
    public class DashboardController : Controller
    {
        private DbTeknoKurEntities Context { get; set; } = new();

        public IActionResult Index()
        {
            ViewBag.ToplamSiparis = Context.TblSiparis.Count();
            ViewBag.ToplamUye = Context.TblUye.Count(x => x.Silik == false);

            var sonSiparisler = Context.TblSiparis
                .OrderByDescending(x => x.SiparisTarihi)
                .Take(5)
                .ToList();

            ViewBag.SonSiparisler = sonSiparisler;

            return View();
        }
    }
}
