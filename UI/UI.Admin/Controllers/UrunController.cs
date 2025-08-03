using System.Data.Entity;
using Layer.Business.Classes.Kategori;
using Layer.Business.Classes.Marka;
using Layer.Business.Classes.Urun;
using Layer.Business.Interfaces.Kategori;
using Layer.Business.Interfaces.Urun;
using Layer.Data;
using Layer.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace UI.Admin.Controllers
{
    public class UrunController : Controller
    {
        // GET: UrunController
        public ActionResult Index()
        {
            TUrun urun = new();
            TResult result = urun.UrunListesi();
            return View(model: result.Veri);
        }

        // GET: UrunController/Details/5
        public ActionResult Detay(int id)
        {
            TUrun urun = new();
            TResult result = urun.UrunDetayGetir(id);
            return View(model: result.Veri.ToListAsync().Result.FirstOrDefault());
        }

        // GET: UrunController/Create
        public ActionResult Yeni()
        {
            TKategori kategoriIslemler = new();
            TMarka markaIslemler = new();

            var kategori = kategoriIslemler.KategoriListesi();
            var marka = markaIslemler.MarkaListesi();

            if (marka.Veri != null)
            {
                ViewBag.Markalar = ((IQueryable<TblMarka>)marka.Veri)
                    .Select(k => new SelectListItem
                    {
                        Text = k.MarkaAdi,
                        Value = k.MarkaId.ToString()
                    }).ToList();
            }
            else
            {
                ViewBag.Markalar = new List<SelectListItem>();
            }
            if (kategori.Veri != null)
            {
                ViewBag.Kategoriler = ((IQueryable<TblKategoriler>)kategori.Veri)
                    .Where(k => k.UstKategoriId.HasValue && k.UstKategoriId > 0) 
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

        // POST: UrunController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Yeni(IFormCollection collection)
        {
            try
            {
                TUrun urunIslemler = new();
                TblUrun urun = new();
                urun.UrunAdi = collection["UrunAdi"];
                urun.UrunKodu = collection["UrunKodu"];
                urun.UrunResimUrl = collection["UrunResimUrl"];
                urun.UrunTaglar = collection["UrunTaglar"];
                urun.UrunFiyat = Convert.ToDecimal(collection["UrunFiyat"]);
                urun.UrunAciklama = collection["UrunAciklama"];
                urun.TeknikOzellikler = collection["TeknikOzellikler"];
                urun.Stok = Convert.ToInt32(collection["Stok"]);
                urun.KategoriId = Convert.ToInt32(collection["KategoriId"]);
                urun.MarkaId = Convert.ToInt32(collection["MarkaId"]);
                urun.Silik = false;
                TResult result = urunIslemler.UrunEkle(urun);

                if (result.Basarili)
                    return RedirectToAction(nameof(Index));
                else
                {
                    ViewBag.ErrorMessage = result.Mesaj;
                    return Yeni();
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Lütfen bilgileri eksiksiz doldurun !!";
                return Yeni();
            }
        }

        // GET: UrunController/Edit/5
        public ActionResult Duzenle(int id)
        {
            TUrun urunIslemler = new();
            TKategori kategoriIslemler = new();
            TMarka markaIslemler = new();

            TResult urunResult = urunIslemler.UrunDetayGetir(id);
            var urun = (urunResult.Veri as IQueryable<TblUrun>)?.FirstOrDefault();

            if (urun == null)
            {
                ViewBag.ErrorMessage = "Düzenlenecek ürün bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            var kategoriListResult = kategoriIslemler.KategoriListesi();
            var markaListResult = markaIslemler.MarkaListesi();

            if (kategoriListResult.Veri is IQueryable<TblKategoriler> tumKategoriler)
            {
                ViewBag.Kategoriler = tumKategoriler
                    .Where(k => k.UstKategoriId.HasValue && k.UstKategoriId > 0)
                    .Select(k => new SelectListItem
                    {
                        Text = k.KategoriAdi,
                        Value = k.KategoriId.ToString(),
                        Selected = (urun.KategoriId == k.KategoriId) 
                    }).ToList();
            }
            else
            {
                ViewBag.Kategoriler = new List<SelectListItem>();
            }

            if (markaListResult.Veri is IQueryable<TblMarka> tumMarkalar)
            {
                ViewBag.Markalar = tumMarkalar
                    .Select(m => new SelectListItem
                    {
                        Text = m.MarkaAdi,
                        Value = m.MarkaId.ToString(),
                        Selected = (urun.MarkaId == m.MarkaId) 
                    }).ToList();
            }
            else
            {
                ViewBag.Markalar = new List<SelectListItem>();
            }

            return View(urun);
        }

        // POST: UrunController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Duzenle(TblUrun urun)
        {
            try
            {
                if (urun.UrunId == 0)
                {
                    ViewBag.ErrorMessage = "Ürün ID'si bulunamadı veya geçersiz.";
                    return Duzenle(urun.UrunId); 
                }

                TUrun urunIslemler = new();
                TResult result = urunIslemler.UrunGuncelle(urun); 

                if (result.Basarili)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewBag.ErrorMessage = result.Mesaj;

                    TKategori kategoriIslemler = new();
                    TMarka markaIslemler = new();

                    var kategoriListResult = kategoriIslemler.KategoriListesi();
                    var markaListResult = markaIslemler.MarkaListesi();

                    if (kategoriListResult.Veri is IQueryable<TblKategoriler> tumKategoriler)
                    {
                        ViewBag.Kategoriler = tumKategoriler
                            .Where(k => k.UstKategoriId.HasValue && k.UstKategoriId > 0)
                            .Select(k => new SelectListItem
                            {
                                Text = k.KategoriAdi,
                                Value = k.KategoriId.ToString(),
                                Selected = (urun.KategoriId == k.KategoriId)
                            }).ToList();
                    }
                    else
                    {
                        ViewBag.Kategoriler = new List<SelectListItem>();
                    }

                    if (markaListResult.Veri is IQueryable<TblMarka> tumMarkalar)
                    {
                        ViewBag.Markalar = tumMarkalar
                            .Select(m => new SelectListItem
                            {
                                Text = m.MarkaAdi,
                                Value = m.MarkaId.ToString(),
                                Selected = (urun.MarkaId == m.MarkaId)
                            }).ToList();
                    }
                    else
                    {
                        ViewBag.Markalar = new List<SelectListItem>();
                    }

                    return View(urun); 
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Ürün güncellenirken bir hata oluştu: " + ex.Message;
                TKategori kategoriIslemler = new();
                TMarka markaIslemler = new();

                var kategoriListResult = kategoriIslemler.KategoriListesi();
                var markaListResult = markaIslemler.MarkaListesi();

                if (kategoriListResult.Veri is IQueryable<TblKategoriler> tumKategoriler)
                {
                    ViewBag.Kategoriler = tumKategoriler
                        .Where(k => k.UstKategoriId.HasValue && k.UstKategoriId > 0)
                        .Select(k => new SelectListItem
                        {
                            Text = k.KategoriAdi,
                            Value = k.KategoriId.ToString(),
                            Selected = (urun.KategoriId == k.KategoriId)
                        }).ToList();
                }
                else
                {
                    ViewBag.Kategoriler = new List<SelectListItem>();
                }

                if (markaListResult.Veri is IQueryable<TblMarka> tumMarkalar)
                {
                    ViewBag.Markalar = tumMarkalar
                        .Select(m => new SelectListItem
                        {
                            Text = m.MarkaAdi,
                            Value = m.MarkaId.ToString(),
                            Selected = (urun.MarkaId == m.MarkaId)
                        }).ToList();
                }
                else
                {
                    ViewBag.Markalar = new List<SelectListItem>();
                }
                return View(urun);
            }
        }

        // GET: UrunController/Delete/5
        public ActionResult Sil(int id)
        {
            TUrun urunIslemler = new();
            TResult result = urunIslemler.UrunDetayGetir(id);
            return View(model: result.Veri.ToListAsync().Result.FirstOrDefault() as TblUrun);
        }

        // POST: UrunController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Sil(int id, IFormCollection collection)
        {
            TUrun urun = new TUrun();
            var result = urun.UrunSil(id); 

            if (result.Basarili)
                return RedirectToAction("Index");
            else
                return View("Hata", result.Mesaj);
        }

    }
}
