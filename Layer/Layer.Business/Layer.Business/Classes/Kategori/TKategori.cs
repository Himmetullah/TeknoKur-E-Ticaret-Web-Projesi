using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Layer.Business.Classes.Ortak;
using Layer.Business.Interfaces.Kategori;
using Layer.Business.Interfaces.Ortak;
using Layer.Data;
using Layer.Entities;

namespace Layer.Business.Classes.Kategori
{
    public class TKategori : IKategori
    {
        public DbTeknoKurEntities Context { get; set; } = new();

        public event IIslemler.RxEklemedenOnce EklemedenOnce;
        public event IIslemler.RxEklediktenSonra EklediktenSonra;
        public event IIslemler.RxSilmedenOnce SilmedenOnce;
        public event IIslemler.RxSildiktenSonra SildiktenSonra;
        public event IIslemler.RxGuncellemedenOnce GuncellemedenOnce;
        public event IIslemler.RxGuncellediktenSonra GuncellediktenSonra;
        public event IIslemler.RxFiltrelemedenOnce FiltrelemedenOnce;
        public event IIslemler.RxFiltrelediktenSonra FiltrelediktenSonra;
        public event IIslemler.RxGirisYapmadanOnce GirisYapmadanOnce;
        public event IIslemler.RxGirisYaptiktanSonra GirisYaptiktanSonra;
        public event IIslemler.RxKayitOlmadanOnce KayitOlmadanOnce;
        public event IIslemler.RxKayitOlduktanSonra KayitOlduktanSonra;
        private TResult result = new();

        public TResult KategoriDetayGetir(int kategoriId)
        {
            result.Veri = Context.TblKategoriler
                .Where(data => data.KategoriId == kategoriId)
                .AsQueryable();
            return result;
        }

        public TResult KategoriEkle(string kategoriAdi, string kategoriSeo, int? ustKategoriId, int ekleyenUyeId, DateTime eklenmeTarihi)
        {
            try
            {
                Context.TblKategoriler.Add(new TblKategoriler
                {
                    KategoriAdi = kategoriAdi,
                    KategoriSeo = TDigerIslemler.SeoOlustur(kategoriAdi),
                    UstKategoriId = ustKategoriId,
                    EkleyenUyeId = ekleyenUyeId,
                    EklenmeTarihi = DateTime.Now
                });
                Context.SaveChanges();
                result.Basarili = true;
            }
            catch (Exception ex)
            {
                result.HataKodu = -1;
                result.Mesaj = "Kategori eklenirken bir hata oluştu: ";
            }
            return result;
        }

        public TResult KategoriEkle(TblKategoriler kategori)
        {
            try
            {
                kategori.Silik = false;
                kategori.KategoriSeo = TDigerIslemler.SeoOlustur(kategori.KategoriAdi);
                kategori.EklenmeTarihi = DateTime.Now;
                Context.TblKategoriler.Add(kategori);
                Context.SaveChanges();
                result.Basarili = true;
            }
            catch (Exception ex)
            {
                result.HataKodu = -1;
                result.Mesaj = "Kategori eklenirken bir hata oluştu: ";
            }
            return result;
        }

        public TResult KategoriFiltrele(int kategoriId, string kategoriAd)
        {
            var kategoriFiltrele = (from data in Context.TblKategoriler
                                    where data.KategoriId == kategoriId || data.KategoriAdi == kategoriAd
                                    select data).AsQueryable();
            result.Veri = kategoriFiltrele;
            return result;
        }

        public TResult KategoriGuncelle(string kategoriAdi, string kategoriSeo, int? ustKategoriId, int guncelleyenUyeId, DateTime guncellenmeTarihi)
        {
            var kategoriGuncelle = (from data in Context.TblKategoriler
                                    where data.KategoriAdi == kategoriAdi && data.KategoriSeo == kategoriSeo 
                                    select data).FirstOrDefault();
            if (kategoriGuncelle != null)
            {
                kategoriGuncelle.KategoriAdi = kategoriAdi;
                kategoriGuncelle.KategoriSeo = kategoriSeo;
                kategoriGuncelle.UstKategoriId = ustKategoriId;
                kategoriGuncelle.GuncelleyenUyeId = guncelleyenUyeId;
                kategoriGuncelle.GuncellemeTarihi = DateTime.Now;
                Context.SaveChanges();
                result.Basarili = true;
            }
            else
                result.Mesaj = "Kategori güncellenirken bir hata oluştu: Kategori bulunamadı.";
            return result;
        }

        public TResult KategoriGuncelle(TblKategoriler kategori)
        {
            var kategoriGuncelle = (from data in Context.TblKategoriler
                                    where data.KategoriId == kategori.KategoriId
                                    select data).FirstOrDefault();
            if (kategoriGuncelle != null)
            {
                kategoriGuncelle.KategoriAdi = kategori.KategoriAdi;
                kategoriGuncelle.KategoriSeo = kategori.KategoriSeo;
                kategoriGuncelle.UstKategoriId = kategori.UstKategoriId;
                kategoriGuncelle.GuncelleyenUyeId = kategori.GuncelleyenUyeId;
                kategoriGuncelle.GuncellemeTarihi = DateTime.Now;
                Context.SaveChanges();
                result.Basarili = true;
            }
            else
                result.Mesaj = "Kategori güncellenirken bir hata oluştu: Kategori bulunamadı.";
            return result;
        }

        public TResult Kategoriler()
        {
            var kategoriler = (from data in Context.TblKategoriler
                               where data.Silik != true 
                               orderby data.KategoriId ascending 
                               select data).AsNoTracking().AsQueryable();
            result.Veri = kategoriler;
            result.Basarili = true; 
            return result;
        }


        public TResult KategoriListesi()
        {
            var kategoriler = Context.TblKategoriler.AsQueryable();
            result.Veri = Context.TblKategoriler.Where(x => x.Silik != true).AsQueryable();
            return result;
        }

        public TResult KategoriSil(int kategoriId)
        {
            var kategoriSil = (from data in Context.TblKategoriler
                                where data.KategoriId == kategoriId
                                select data).FirstOrDefault();
            if (kategoriSil != null)
            {
                kategoriSil.Silik = true;
                Context.SaveChanges();
                result.Basarili = true;
            }
            else
                result.Mesaj = "Silinecek kategori bulunamadı.";
            return result;
        }
        public List<TblKategoriler> KategoriListesiGetir()
        {
            using var db = new DbTeknoKurEntities();
            return db.TblKategoriler.Where(x => x.Silik == false || x.Silik == null)
             .ToList();
        }
    }
}
