using Layer.Business.Classes.Kategori;
using Layer.Business.Classes.Urun;
using Layer.Data;
using Microsoft.AspNetCore.Mvc;

namespace UI.Customer.Controllers
{
    public class KendinToplaController : Controller
    {
        private TUrun urunIslemleri = new();
        private TKategori kategoriIslemleri = new();
        private DbTeknoKurEntities Context = new();

        public IActionResult Index()
        {
            List<TblKategoriler> anaKategoriler = new List<TblKategoriler>();

            var tumKategoriler = kategoriIslemleri.KategoriListesiGetir();

            if (tumKategoriler != null)
            {
                anaKategoriler = tumKategoriler
                                    .Where(k => k.Silik != true && k.UstKategoriId == 2)
                                    .ToList();
            }

            return View(anaKategoriler);
        }
        public IActionResult GetUrunlerByKategori(int kategoriId)
        {
            var urunResult = urunIslemleri.Urunler();

            List<TblUrun> urunler = new List<TblUrun>();

            if (urunResult.Basarili && urunResult.Veri != null)
            {
                urunler = (urunResult.Veri as IQueryable<TblUrun>)
                                .Where(u => u.Silik != true && u.KategoriId == kategoriId)
                                .ToList();
            }

            return PartialView("_UrunListesiPartial", urunler);
        }
        [HttpPost]
        public IActionResult EkleToplamaUrunleri()
        {
            var session = Request.Cookies["UserSession"];
            if (session == null)
                return Unauthorized("Giriş yapmanız gerekmektedir.");

            int userId = Convert.ToInt32(session);

            try
            {
                var form = Request.Form;

                int index = 0;
                while (form.ContainsKey($"urunler[{index}][id]"))
                {
                    int urunId = Convert.ToInt32(form[$"urunler[{index}][id]"]);
                    decimal fiyat = Convert.ToDecimal(form[$"urunler[{index}][price]"]);

                    TblSepet sepet = new TblSepet
                    {
                        UyeId = userId,
                        UrunId = urunId,
                        Adet = 1,
                        EklenmeTarihi = DateTime.Now
                    };

                    Context.TblSepet.Add(sepet);
                    index++;
                }

                Context.SaveChanges();
                return Ok("Sepete başarıyla eklendi.");
            }
            catch (Exception ex)
            {
                return BadRequest("Hata oluştu: " + ex.Message);
            }
        }
    }
}
