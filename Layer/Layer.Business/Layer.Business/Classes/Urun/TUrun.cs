using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Layer.Business.Classes.Ortak;
using Layer.Business.Interfaces.Ortak;
using Layer.Business.Interfaces.Urun;
using Layer.Data;
using Layer.Entities;

namespace Layer.Business.Classes.Urun
{
    public class TUrun : IUrun
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

        public TResult UrunDetayGetir(int urunId)
        {
            var result = new TResult();
            result.Veri = Context.TblUrun
                .Where(data => data.UrunId == urunId)
                .AsQueryable();
            return result;
        }

        public TResult UrunEkle(string urunAdi, string urunAciklama, string urunResimUrl, decimal urunFiyati, string urunKodu, int kategoriId, int markaId, string urunSeo, string urunTaglar, string urunTeknikOzellikler, int urunStok, int EkleyenUyeId, DateTime EklenmeTarihi)
        {
            var result = new TResult();
            try
            {
                Context.TblUrun.Add(new TblUrun
                {
                    UrunAdi = urunAdi,
                    UrunAciklama = urunAciklama,
                    UrunResimUrl = urunResimUrl,
                    UrunFiyat = urunFiyati,
                    UrunKodu = urunKodu,
                    KategoriId = kategoriId,
                    MarkaId = markaId,
                    UrunSeo = urunSeo,
                    UrunTaglar = urunTaglar,
                    TeknikOzellikler = urunTeknikOzellikler,
                    Stok = urunStok,
                    EkleyenUyeId = EkleyenUyeId,
                    EklenmeTarihi = EklenmeTarihi
                });
                Context.SaveChanges();
                result.Basarili = true;
            }
            catch (Exception ex)
            {
                result.Basarili = false;
                result.Mesaj = "Ürün eklenirken bir hata oluştu: " + ex.Message;
            }
            return result;
        }

        public TResult UrunEkle(TblUrun urun)
        {
            var result = new TResult();
            try
            {
                urun.Silik = false;
                urun.UrunSeo = TDigerIslemler.SeoOlustur(urun.UrunAdi);
                urun.EklenmeTarihi = DateTime.Now;
                Context.TblUrun.Add(urun);
                Context.SaveChanges();
                result.Basarili = true;
            }
            catch (Exception ex)
            {
                result.Basarili = false;
                result.Mesaj = "Ürün eklenirken bir hata oluştu: " + ex.Message;
            }
            return result;
        }

        public TResult UrunFiltrele(int urunId, string urunAd, string urunKodu)
        {
            var result = new TResult();
            try
            {
                result.Veri = Context.TblUrun.Where(data => urunId == data.UrunId || urunAd == data.UrunAdi || urunKodu == data.UrunKodu).AsQueryable();
                result.Basarili = true;
            }
            catch (Exception ex)
            {
                result.Basarili = false;
                result.Mesaj = "Ürünler filtrelenirken bir hata oluştu: " + ex.Message;
            }
            return result;
        }

        public TResult UrunGuncelle(string urunAdi, string urunAciklama, string urunResimUrl, decimal urunFiyati, string urunKodu, int kategoriId, int markaId, string urunSeo, string urunTaglar, string urunTeknikOzellikler, int urunStok, int GuncelleyenUyeId, DateTime GuncellenmeTarihi)
        {
            var result = new TResult();
            try
            {
                var urunGuncelle = Context.TblUrun.FirstOrDefault(data =>
                    data.UrunAdi == urunAdi && data.UrunAciklama == urunAciklama && data.UrunResimUrl == urunResimUrl &&
                    data.UrunFiyat == urunFiyati && data.UrunKodu == urunKodu && data.KategoriId == kategoriId &&
                    data.MarkaId == markaId && data.UrunSeo == urunSeo && data.UrunTaglar == urunTaglar &&
                    data.TeknikOzellikler == urunTeknikOzellikler && data.Stok == urunStok);
                if (urunGuncelle != null)
                {
                    urunGuncelle.UrunAdi = urunAdi;
                    urunGuncelle.UrunAciklama = urunAciklama;
                    urunGuncelle.UrunResimUrl = urunResimUrl;
                    urunGuncelle.UrunFiyat = urunFiyati;
                    urunGuncelle.UrunKodu = urunKodu;
                    urunGuncelle.KategoriId = kategoriId;
                    urunGuncelle.MarkaId = markaId;
                    urunGuncelle.UrunSeo = urunSeo;
                    urunGuncelle.UrunTaglar = urunTaglar;
                    urunGuncelle.TeknikOzellikler = urunTeknikOzellikler;
                    urunGuncelle.Stok = urunStok;
                    urunGuncelle.GuncelleyenUyeId = GuncelleyenUyeId;
                    urunGuncelle.GuncellemeTarihi = DateTime.Now;
                    Context.SaveChanges();
                    result.Basarili = true;
                }
                else
                {
                    result.Basarili = false;
                    result.Mesaj = "Güncellenecek ürün bulunamadı.";
                }
            }
            catch (Exception ex)
            {
                result.Basarili = false;
                result.Mesaj = "Ürün güncellenirken bir hata oluştu: " + ex.Message;
            }
            return result;
        }

        public TResult UrunGuncelle(TblUrun urun)
        {
            var result = new TResult();
            try
            {
                var urunGuncelle = Context.TblUrun.FirstOrDefault(data => data.UrunId == urun.UrunId);
                if (urunGuncelle != null)
                {
                    urunGuncelle.UrunAdi = urun.UrunAdi;
                    urunGuncelle.UrunAciklama = urun.UrunAciklama;
                    urunGuncelle.UrunResimUrl = urun.UrunResimUrl;
                    urunGuncelle.UrunFiyat = urun.UrunFiyat;
                    urunGuncelle.UrunKodu = urun.UrunKodu;
                    urunGuncelle.KategoriId = urun.KategoriId;
                    urunGuncelle.MarkaId = urun.MarkaId;
                    urunGuncelle.UrunSeo = urun.UrunSeo;
                    urunGuncelle.UrunTaglar = urun.UrunTaglar;
                    urunGuncelle.TeknikOzellikler = urun.TeknikOzellikler;
                    urunGuncelle.Stok = urun.Stok;
                    urunGuncelle.GuncelleyenUyeId = urun.GuncelleyenUyeId;
                    urunGuncelle.GuncellemeTarihi = DateTime.Now;
                    Context.SaveChanges();
                    result.Basarili = true;
                }
                else
                {
                    result.Basarili = false;
                    result.Mesaj = "Güncellenecek ürün bulunamadı.";
                }
            }
            catch (Exception ex)
            {
                result.Basarili = false;
                result.Mesaj = "Ürün güncellenirken bir hata oluştu: " + ex.Message;
            }
            return result;
        }

        public TResult Urunler()
        {
            var result = new TResult();
            try
            {
                result.Veri = Context.TblUrun.Where(data => data.Silik != true).OrderBy(data => data.UrunId).AsQueryable();
                result.Basarili = true;
            }
            catch (Exception ex)
            {
                result.Basarili = false;
                result.Mesaj = "Ürünler listelenirken bir hata oluştu: " + ex.Message;
            }
            return result;
        }

        public TResult UrunListesi()
        {
            var result = new TResult();
            try
            {
                var urun = (from data in Context.TblUrun where data.Silik != true select data).AsQueryable();
                result.Veri = urun;
                result.Basarili = true;
            }
            catch (Exception ex)
            {
                result.Basarili = false;
                result.Mesaj = "Ürün listesi alınırken bir hata oluştu: " + ex.Message;
            }
            return result;
        }

        public TResult UrunSil(int urunId)
        {
            var result = new TResult();
            try
            {
                var urunSil = Context.TblUrun.FirstOrDefault(data => data.UrunId == urunId);
                if (urunSil != null)
                {
                    urunSil.Silik = true;
                    Context.SaveChanges();
                    result.Basarili = true;
                }
                else
                {
                    result.Basarili = false;
                    result.Mesaj = "Silinecek ürün bulunamadı.";
                }
            }
            catch (Exception ex)
            {
                result.Basarili = false;
                result.Mesaj = "Ürün silinirken bir hata oluştu: " + ex.Message;
            }
            return result;
        }
    }
}
