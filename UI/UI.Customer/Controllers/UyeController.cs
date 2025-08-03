using Layer.Business.Classes.Uye;
using Layer.Business.Interfaces.Ortak;
using Layer.Data;
using Layer.Entities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace UI.Customer.Controllers
{
    public class UyeController : Controller
    {
        private DbTeknoKurEntities Context { get; set; } = new();

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult KayitOl()
        {
            var userSession = Request.Cookies["UserSession"];
            if (userSession != null)
            {
                return RedirectToAction("Detay", "Uye");
            }
            return View();
        }
        public IActionResult GirisYap()
        {
            var userSession = Request.Cookies["UserSession"];
            if (userSession != null)
            {
                return RedirectToAction("Detay", "Uye");
            }
            return View();
        }
        public IActionResult Detay()
        {
            var userId = Request.Cookies["UserSession"];
            if (userId == null)
                return RedirectToAction("GirisYap");

            int UserId = Convert.ToInt32(userId);

            var uye = (from data in Context.VwUyeKisi
                       where data.UyeId == UserId
                       select data).FirstOrDefault();
            ViewBag.Ad = uye?.KisiAd;
            ViewBag.Soyad = uye?.KisiSoyad;
            ViewBag.Mail = uye?.UyeMailAdresi;

            var adres = (from data in Context.TblKisiAdres where data.KisiId == UserId select data).FirstOrDefault();
            var telefon = (from data in Context.TblKisiTelefon where data.KisiId == UserId select data).FirstOrDefault();

            ViewBag.Adres = adres;
            ViewBag.Telefon = telefon;

            var siparisler = Context.TblSiparis
                .Where(s => s.UyeId == UserId)
                .OrderByDescending(s => s.SiparisTarihi)
                .Select(s => new 
                {
                    s.SiparisId,
                    s.SiparisTarihi,
                    s.ToplamTutar,
                    s.OdemeDurumu,
                    AdresDetay = s.TblKisiAdres != null ? s.TblKisiAdres.Adres : "Adres Bilgisi Yok"
                })
                .ToList();

            ViewBag.Siparisleriniz = siparisler;

            return View();
        }
        [HttpPost]
        public IActionResult KayitOl(string Sifre, string Mail, string Ad, string Soyad, int Cinsiyet, string DogumTarihi, string Tc)
        {
            var r = Request;
            TblUye Uye = new();
            Uye.Sifre = Sifre;
            Uye.UyeMailAdresi = Mail;

            TblUyeKisi Kisi = new();
            Kisi.KisiAd = Ad;
            Kisi.KisiSoyad = Soyad;
            Kisi.KisiCinsiyet = Cinsiyet.ToString();
            Kisi.KisiDogumTarihi = Convert.ToDateTime(DogumTarihi);
            Kisi.KisiTc = Convert.ToInt64(Tc);
            TUye uye = new();
            TResult result = uye.KayitOl(Uye, Kisi);

            ViewBag.Mesaj = result.Basarili ? "Kayıt başarılı. Giriş yapabilirsiniz." : "Hata oluştu: " + result.Mesaj;
            return View();
        }
        [HttpPost]
        public IActionResult GirisYap(string Mail, string Sifre)
        {
            TUye uyeManager = new();

            TblUye uye = new TblUye
            {
                UyeMailAdresi = Mail,
                Sifre = Sifre
            };

            TResult result = uyeManager.GirisYap(uye);

            if (result.Basarili && result.Data != null)
            {
                dynamic DynData = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(result.Data));

                string AdSoyad = $"{DynData.KisiAd} {DynData.KisiSoyad}";
                string UyeId = DynData.UyeId.ToString();

                Response.Cookies.Append("UserSession", UyeId, new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(1),
                    HttpOnly = false,
                    Path = "/"
                });

                Response.Cookies.Append("NameSurname", AdSoyad, new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(1),
                    HttpOnly = false,
                    Path = "/"
                });

                return RedirectToAction("Index", "Home");
            }
            ViewBag.Mesaj = result.Mesaj ?? "Giriş başarısız. Lütfen bilgilerinizi kontrol edin.";
            return View();
        }

        [HttpPost]
        public IActionResult IletisimKaydetGuncelle(string AdresBaslik, string AdresUlke, string AdresIl, string AdresIlce, string Adres, bool FaturaAdresi, string Telefon)
        {
            Console.WriteLine(">> IletisimKaydetGuncelle METODUNA GİRİLDİ");

            var userId = Request.Cookies["UserSession"];
            if (userId == null)
                return RedirectToAction("GirisYap");

            int UserId = Convert.ToInt32(userId);

            var adres = (from data in Context.TblKisiAdres where data.KisiId == UserId select data).FirstOrDefault();
            var telefon = (from data in Context.TblKisiTelefon where data.KisiId == UserId select data).FirstOrDefault();

            try
            {
                if (adres == null)
                {
                    adres = new TblKisiAdres { KisiId = UserId };
                    Context.TblKisiAdres.Add(adres);
                }

                adres.AdresBaslik = AdresBaslik;
                adres.AdresUlke = AdresUlke;
                adres.AdresIl = AdresIl;
                adres.AdresIlce = AdresIlce;
                adres.Adres = Adres;
                adres.FaturaAdresi = FaturaAdresi;

                if (telefon == null)
                {
                    telefon = new TblKisiTelefon { KisiId = UserId };
                    Context.TblKisiTelefon.Add(telefon);
                }

                telefon.TelefonNumarasi = Telefon;

                Context.SaveChanges();
            }
            catch (Exception ex)
            {
                string detay = ex.ToString(); // tüm exception zincirini alır
                return Content("Hata oluştu:<br><br>" + detay);
            }
            return RedirectToAction("Detay");
        }

        public IActionResult CikisYap()
        {
            Response.Cookies.Delete("UserSession");
            Response.Cookies.Delete("NameSurname");
            return RedirectToAction("Index", "Home");
        }
    }
}
