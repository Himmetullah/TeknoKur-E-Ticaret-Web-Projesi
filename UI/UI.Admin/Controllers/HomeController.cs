using System.Data.Entity;
using Layer.Business.Classes.Uye;
using Layer.Data;
using Layer.Entities;
using Microsoft.AspNetCore.Mvc;

namespace UI.Admin.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GirisYap()
        {
            TUye uyelikIslemleri = new();
            TblUye uye = new();
            uye.UyeMailAdresi = Request.Form["GirisBilgileri"][0].ToString();
            uye.Sifre = Request.Form["GirisBilgileri"][1].ToString();
            TResult result = uyelikIslemleri.GirisYap(uye);

            if (result.Basarili)
            {
                try
                {
                    dynamic data = result.Data;

                    int uyeId = Convert.ToInt32(data.GetType().GetProperty("UyeId")?.GetValue(data));
                    string kisiAd = data.GetType().GetProperty("KisiAd")?.GetValue(data)?.ToString() ?? "";
                    string kisiSoyad = data.GetType().GetProperty("KisiSoyad")?.GetValue(data)?.ToString() ?? "";

                    // Session kullanımı
                    HttpContext.Session.SetInt32("AdminUyeId", uyeId);
                    HttpContext.Session.SetString("AdminAdSoyad", $"{kisiAd} {kisiSoyad}");

                    return RedirectToAction("Index", "Dashboard");
                }
                catch (Exception ex)
                {
                    ViewBag.Mesaj = "Giriş sırasında bir hata oluştu: " + ex.Message;
                    return View("Index");
                }
            }

            ViewBag.Mesaj = result.Mesaj ?? "Giriş yapılamadı.";
            return View("Index");

        }
    }
}
