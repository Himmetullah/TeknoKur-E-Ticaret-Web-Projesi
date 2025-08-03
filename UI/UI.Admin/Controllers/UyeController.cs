using System.Data.Entity;
using Layer.Business.Classes.Uye;
using Layer.Data;
using Layer.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace UI.Admin.Controllers
{
    public class UyeController : Controller
    {
        private DbTeknoKurEntities Context { get; set; } = new();
        // GET: UyeController
        public ActionResult Index()
        {
            var uyeler = Context.VwUyeKisi.Where(x => x.Silik == false).ToList();
            return View(uyeler);
        }

        // GET: UyeController/Details/5
        public ActionResult Detay(int id)
        {
            var uyeler = Context.VwUyeKisi.FirstOrDefault(x => x.UyeId == id);
            return View(uyeler);
        }

        // GET: UyeController/Create
        public ActionResult Yeni()
        {
            return View();
        }

        // POST: UyeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Yeni(IFormCollection collection)
        {
            try
            {
                TUye uye = new();
                TblUye uyeler = new();

                uyeler.UyeMailAdresi = collection["UyeMailAdresi"];
                uyeler.Sifre = collection["UyeSifre"];
                uyeler.Admin = collection["UyeAdmin"] == "on";
                uyeler.UyeKayitTarihi = DateTime.Now;
                uyeler.Silik = false;

                TblUyeKisi uyeKisi = new();
                uyeKisi.KisiAd = collection["UyeKisiAd"];
                uyeKisi.KisiSoyad = collection["UyeKisiSoyad"];
                uyeKisi.KisiTc = Convert.ToInt64(collection["UyeKisiTc"]);
                uyeKisi.KisiDogumTarihi = Convert.ToDateTime(collection["UyeKisiDogumTarihi"]);
                uyeKisi.KisiCinsiyet = collection["UyeKisiCinsiyet"];
                uyeKisi.Silik = false;

                TResult result = uye.KayitOl(uyeler, uyeKisi);

                if (result.Basarili)
                    return RedirectToAction("Index");

                ViewBag.ErrorMessage = result.Mesaj;
                return View();
            }
            catch
            {
                ViewBag.ErrorMessage = "Lütfen bilgileri eksiksiz ve doğru giriniz!";
                return View();
            }
        }

        [HttpGet]
        public ActionResult Duzenle(int id)
        {
            var uyeKisi = Context.VwUyeKisi.FirstOrDefault(x => x.UyeId == id && x.Silik == false);

            if (uyeKisi == null)
            {
                ViewBag.ErrorMessage = "Kullanıcı bulunamadı!";
                return RedirectToAction("Index");
            }

            return View(uyeKisi);
        }

        // POST: UyeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Duzenle(int id, IFormCollection form)
        {
            try
            {
                TUye uyeIslemleri = new();

                var uye = uyeIslemleri.Context.TblUye.FirstOrDefault(x => x.UyeId == id);
                var kisi = uyeIslemleri.Context.TblUyeKisi.FirstOrDefault(x => x.UyeId == id);

                if (uye == null || kisi == null)
                {
                    TempData["ErrorMessage"] = "Kullanıcı bulunamadı!";
                    return RedirectToAction("Index");
                }

                uye.UyeMailAdresi = form["UyeMailAdresi"];
                uye.Sifre = form["Sifre"];
                uye.Admin = form["Admin"].Count > 0 && form["Admin"][0] == "on";

                kisi.KisiAd = form["KisiAd"];
                kisi.KisiSoyad = form["KisiSoyad"];
                kisi.KisiTc = Convert.ToInt64(form["KisiTc"]);
                kisi.KisiDogumTarihi = Convert.ToDateTime(form["KisiDogumTarihi"]);
                kisi.KisiCinsiyet = form["KisiCinsiyet"];

                uyeIslemleri.Context.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.ErrorMessage = "Bir hata oluştu!";
                var uyeKisi = Context.VwUyeKisi.FirstOrDefault(x => x.UyeId == id);
                return View(uyeKisi);
            }
        }
        // GET: UyeController/Delete/5
        public ActionResult Sil(int id)
        {
            var uye = Context.VwUyeKisi.FirstOrDefault(x => x.UyeId == id);
            if (uye == null)
                return NotFound();

            return View(uye);
        }

        // POST: UyeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Sil(int id, IFormCollection collection)
        {
            try
            {
                TUye uyeIslemleri = new();
                var uye = uyeIslemleri.Context.TblUye.FirstOrDefault(x => x.UyeId == id);
                var uyeKisi = uyeIslemleri.Context.TblUyeKisi.FirstOrDefault(x => x.UyeId == id);

                if (uye == null)
                    return NotFound();

                uye.Silik = true;

                if (uyeKisi != null)
                    uyeKisi.Silik = true;

                uyeIslemleri.Context.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Hata = "Silme işlemi sırasında hata oluştu!";
                return View();
            }
        }

        public ActionResult Bilgilerim(int id)
        {
            int? uyeId = null;
            if (Request.Cookies["UserSession"] != null && int.TryParse(Request.Cookies["UserSession"], out int parsedUyeId))
            {
                uyeId = parsedUyeId;
            }

            if (!uyeId.HasValue)
            {
                return RedirectToAction("GirisYap", "Uye"); // veya uygun bir giriş sayfası
            }

            var uyeKisi = Context.VwUyeKisi.FirstOrDefault(x => x.UyeId == uyeId.Value && x.Silik == false);

            if (uyeKisi == null)
            {
                ViewBag.ErrorMessage = "Kullanıcı bulunamadı!";
                return RedirectToAction("Index", "Home");
            }

            return View(uyeKisi);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CikisYap()
        {
            Response.Cookies.Delete("UserSession", new CookieOptions { Path = "/" });
            Response.Cookies.Delete("NameSurname", new CookieOptions { Path = "/" });
            Response.Cookies.Delete("UyeId", new CookieOptions { Path = "/" });

            return RedirectToAction("Index", "Home");
        }

    }
}