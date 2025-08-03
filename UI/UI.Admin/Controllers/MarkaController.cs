using System.Data.Entity;
using Layer.Business.Classes.Kategori;
using Layer.Business.Classes.Marka;
using Layer.Data;
using Layer.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace UI.Admin.Controllers
{
    public class MarkaController : Controller
    {
        // GET: MarkaController
        public ActionResult Index()
        {
            TMarka marka = new();
            TResult result = marka.MarkaListesi();
            return View(model: result.Veri);
        }

        // GET: MarkaController/Details/5
        public ActionResult Detay(int id)
        {
            TMarka marka = new();
            TResult result = marka.MarkaDetayGetir(id);
            return View(model: result.Veri.ToListAsync().Result.FirstOrDefault());
        }

        // GET: MarkaController/Create
        public ActionResult Yeni()
        {
            TMarka markaIslemler = new();
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

            return View();
        }

        // POST: MarkaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Yeni(IFormCollection collection)
        {
            try
            {
                TMarka marka = new();
                TblMarka markalar = new();
                markalar.MarkaAdi = collection["MarkaAdi"];
                markalar.Silik = false;
                TResult result = marka.MarkaEkle(markalar);

                if (result.Basarili)
                    return RedirectToAction(nameof(Index));
                else
                {
                    ViewBag.ErrorMessage = result.Mesaj;
                    return Yeni();
                }
            }
            catch
            {
                ViewBag.ErrorMessage = "Lütfen bilgileri eksiksiz doldurun !!";
                return Yeni();
            }
        }

        // GET: MarkaController/Edit/5
        public ActionResult Duzenle(int id)
        {
            TMarka markaIslemler = new();
            TResult result = markaIslemler.MarkaDetayGetir(id);

            var marka = (result.Veri as IQueryable<TblMarka>)?.FirstOrDefault();

            if (marka == null)
            {
                ViewBag.ErrorMessage = "Kategori bulunamadı.";
                return RedirectToAction(nameof(Index));
            }
            else
                ViewBag.Markalar = new List<SelectListItem>();
            return View(marka);
        }

        // POST: MarkaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Duzenle(TblMarka markalar)
        {
            try
            {
                if (markalar.MarkaId == 0)
                {
                    ViewBag.ErrorMessage = "Marka ID bulunamadı.";
                    return Duzenle(markalar.MarkaId);
                }
                TMarka markaIslemler = new();
                TResult result = markaIslemler.MarkaGuncelle(markalar);

                if (result.Basarili)
                    return RedirectToAction(nameof(Index));
                else
                {
                    var marka = markaIslemler.MarkaListesi();

                    ViewBag.Markalar = ((IQueryable<TblMarka>)marka.Veri)
                   .Select(k => new SelectListItem
                   {
                       Text = k.MarkaAdi,
                       Value = k.MarkaId.ToString()
                   }).ToList();
                }
                return View(markalar);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Marka güncellenirken bir hata oluştu: ";
                return View(markalar);
            }
        }

        // GET: MarkaController/Delete/5
        public ActionResult Sil(int id)
        {
            TMarka markaIslemler = new();
            TResult result = markaIslemler.MarkaDetayGetir(id);
            return View(model: result.Veri.ToListAsync().Result.FirstOrDefault() as TblMarka);
        }

        // POST: MarkaController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Sil(int id, IFormCollection collection)
        {
            TMarka markaIslemler = new();
            TResult result = markaIslemler.MarkaSil(id);

            if (result.Basarili)
                return RedirectToAction("Index");
            else
              return View("Hata", result.Mesaj);
        }
    }
}
