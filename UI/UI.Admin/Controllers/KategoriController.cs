using System.Data.Entity;
using Layer.Business.Classes.Kategori;
using Layer.Business.Interfaces.Kategori;
using Layer.Data;
using Layer.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace UI.Admin.Controllers
{
    public class KategoriController : Controller
    {
        // GET: KategoriController
        public ActionResult Index()
        {
            TKategori kategori = new();
            TResult result = kategori.KategoriListesi();
            return View(model: result.Veri);
        }

        // GET: KategoriController/Details/5
        public ActionResult Detay(int id)
        {
            TKategori kategori = new();
            TResult result = kategori.KategoriDetayGetir(id);
            return View(model: result.Veri.ToListAsync().Result.FirstOrDefault());
        }

        // GET: KategoriController/Create
        public ActionResult Yeni()
        {
            TKategori kategoriIslemler = new();

            var kategori = kategoriIslemler.KategoriListesi();

            if (kategori.Veri != null)
            {
                ViewBag.UstKategoriler = ((IQueryable<TblKategoriler>)kategori.Veri)
                    .Where(k => k.UstKategoriId == null && k.Silik != true)
                    .Select(k => new SelectListItem
                    {
                        Text = k.KategoriAdi,
                        Value = k.KategoriId.ToString()
                    }).ToList();
            }
            else
            {
                ViewBag.Kategoriler = new List<SelectListItem>();
            }
            return View();
        }

        // POST: KategoriController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Yeni(IFormCollection collection)
        {
            try
            {
                string kategoriAdi = collection["KategoriAdi"];
                string ustKategoriIdStr = collection["UstKategoriId"];

                if (string.IsNullOrWhiteSpace(kategoriAdi))
                {
                    ViewBag.ErrorMessage = "Lütfen tüm alanları eksiksiz doldurun !!";
                    return Yeni();
                }

                TKategori kategoriIslemler = new();
                TblKategoriler kategori = new();
                kategori.KategoriAdi = kategoriAdi;
                kategori.UstKategoriId = string.IsNullOrEmpty(ustKategoriIdStr) ? (int?)null : Convert.ToInt32(ustKategoriIdStr);
                kategori.Silik = false;

                TResult result = kategoriIslemler.KategoriEkle(kategori);

                if (result.Basarili)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewBag.ErrorMessage = result.Mesaj ?? "Kategori eklenemedi.";
                    return Yeni();
                }
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "Bir hata oluştu. Lütfen bilgileri kontrol edin.";
                return Yeni();
            }
        }

        // GET: KategoriController/Edit/5
        public ActionResult Duzenle(int id)
        {
            TKategori kategoriIslemler = new();
            TResult result = kategoriIslemler.KategoriDetayGetir(id);

            var kategori = (result.Veri as IQueryable<TblKategoriler>)?.FirstOrDefault();

            if (kategori == null)
            {
                ViewBag.ErrorMessage = "Kategori bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            var kategoriListResult = kategoriIslemler.KategoriListesi();
            if (kategoriListResult.Veri is IQueryable<TblKategoriler> tumKategoriler)
            {
                ViewBag.Kategoriler = tumKategoriler
                    .Where(k => k.UstKategoriId.HasValue && k.UstKategoriId > 0)
                    .Select(k => new SelectListItem
                    {
                        Text = k.KategoriAdi,
                        Value = k.KategoriId.ToString(),
                        Selected = (kategori.KategoriId == k.KategoriId)
                    }).ToList();
            }
            else
            {
                ViewBag.Kategoriler = new List<SelectListItem>();
            }
            return View(kategori);
        }

        // POST: KategoriController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Duzenle(TblKategoriler kategori)
        {
            try
            {
                if (kategori.KategoriId == 0)
                {
                    ViewBag.ErrorMessage = "Ürün ID'si bulunamadı veya geçersiz.";
                    return Duzenle(kategori.KategoriId);
                }
                TKategori kategoriIslemler = new();
                TResult result = kategoriIslemler.KategoriGuncelle(kategori);

                if (result.Basarili)
                    return RedirectToAction(nameof(Index));
                else
                {
                    var kategoriListResult = kategoriIslemler.KategoriListesi();

                    if (kategoriListResult.Veri is IQueryable<TblKategoriler> tumKategoriler)
                    {
                        ViewBag.Kategoriler = tumKategoriler
                            .Where(k => k.UstKategoriId.HasValue && k.UstKategoriId > 0)
                            .Select(k => new SelectListItem
                            {
                                Text = k.KategoriAdi,
                                Value = k.KategoriId.ToString(),
                                Selected = (kategori.KategoriId == k.KategoriId)
                            }).ToList();
                    }
                }
                return View(kategori);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Ürün güncellenirken bir hata oluştu: " + ex.Message;
                return View(kategori);
            }
        }

        // GET: KategoriController/Delete/5
        public ActionResult Sil(int id)
        {
            TKategori kategoriIslemler = new();
            TResult result = kategoriIslemler.KategoriDetayGetir(id);
            return View(model: result.Veri.ToListAsync().Result.FirstOrDefault() as TblKategoriler);
        }

        // POST: KategoriController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Sil(int id, IFormCollection collection)
        {
            TKategori kategoriIslemler = new();
            var result = kategoriIslemler.KategoriSil(id);

            if (result.Basarili)
                return RedirectToAction("Index");
            else
                return View("Hata", result.Mesaj);
        }
    }
}