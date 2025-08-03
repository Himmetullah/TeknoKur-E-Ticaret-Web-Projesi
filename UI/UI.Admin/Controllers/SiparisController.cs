using System.Data.Entity;
using Layer.Data;
using Microsoft.AspNetCore.Mvc;

namespace UI.Admin.Controllers
{
    public class SiparisController : Controller
    {
        private DbTeknoKurEntities Context { get; set; } = new();
        public IActionResult Index()
        {
            var siparisler = Context.TblSiparis
                .OrderByDescending(x => x.SiparisTarihi)
                .ToList();

            return View(siparisler);
        }
        public IActionResult Detay(int id)
        {
            var siparis = Context.TblSiparis
                .Where(s => s.SiparisId == id)
                .Include(s => s.TblSiparisDetay) 
                .FirstOrDefault();

            if (siparis == null)
            {
                return NotFound(); 
            }

            return View(siparis);
        }
    }
}