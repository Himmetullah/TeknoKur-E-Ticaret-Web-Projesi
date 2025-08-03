using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Layer.Business.Classes.Ortak;
using Layer.Business.Interfaces.Marka;
using Layer.Business.Interfaces.Ortak;
using Layer.Data;
using Layer.Entities;

namespace Layer.Business.Classes.Marka
{
    public class TMarka : IMarka
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

        public TResult MarkaDetayGetir(int markaId)
        {
            var markaDetay = (from data in Context.TblMarka
                              where data.MarkaId == markaId 
                              select data).AsQueryable();
            result.Veri = markaDetay;
            return result;

        }

        public TResult MarkaEkle(string markaAdi, string markaSeo, int ekleyenUyeId, DateTime eklenmeTarihi)
        {
            try
            {
                Context.TblMarka.Add(new TblMarka
                {
                    MarkaAdi = markaAdi,
                    MarkaSeo = markaSeo,
                    EkleyenUyeId = ekleyenUyeId,
                    EklenmeTarihi = DateTime.Now,
                });
                Context.SaveChanges();
                result.Basarili = true;
            }
            catch (Exception ex)
            {
                result.HataKodu = -1;
                result.Mesaj = "Marka eklenirken bir hata oluştu: ";
            }
            return result;
        }

        public TResult MarkaFiltrele(int markaId, string markaAd)
        {
            var markaFiltre = (from data in Context.TblMarka
                              where data.MarkaId == markaId || data.MarkaAdi == markaAd
                              select data).AsQueryable();
            result.Veri = markaFiltre;
            return result;
        }

        public TResult MarkaGuncelle(string markaAdi, string markaSeo, int guncelleyenUyeId, DateTime guncellenmeTarihi)
        {
            var markaGuncelle = (from data in Context.TblMarka
                                 where data.MarkaAdi == markaAdi && data.MarkaSeo == markaSeo
                                 select data).FirstOrDefault();
            if (markaGuncelle != null)
            {
                markaGuncelle.MarkaAdi = markaAdi;
                markaGuncelle.MarkaSeo = markaSeo;
                markaGuncelle.GuncelleyenUyeId = guncelleyenUyeId;
                markaGuncelle.GuncellemeTarihi = DateTime.Now;
                Context.SaveChanges();
                result.Basarili = true;
            }
            else
                result.Mesaj = "Marka güncellenirken bir hata oluştu: ";
            return result;
        }

        public TResult Markalar()
        {
            var markalar = (from data in Context.TblMarka
                            where data.Silik != true
                            orderby data.MarkaId ascending
                            select data).AsQueryable();
            result.Veri = markalar;
            result.Basarili = true;
            return result;
        }

        public TResult MarkaListesi()
        {
            var markaListesi = Context.TblMarka.AsQueryable();
            result.Veri = Context.TblMarka.Where(m => m.Silik != true).AsQueryable();
            return result;
        }

        public TResult MarkaSil(int markaId)
        {
            var markaSil = (from data in Context.TblMarka
                            where data.MarkaId == markaId
                            select data).FirstOrDefault();
            if (markaSil != null)
            {
                markaSil.Silik = true;
                Context.SaveChanges();
                result.Basarili = true;
            }
            else
                result.Mesaj = "Marka silinirken bir hata oluştu: ";
            return result;
        }
        public TResult MarkaEkle(TblMarka marka)
        {
            try
            {
                Context.TblMarka.Add(new TblMarka
                {
                    MarkaAdi = marka.MarkaAdi,
                    MarkaSeo = TDigerIslemler.SeoOlustur(marka.MarkaAdi),
                    EkleyenUyeId = marka.EkleyenUyeId,
                    EklenmeTarihi = DateTime.Now,
                });
                Context.SaveChanges();
                result.Basarili = true;
            }
            catch (Exception ex)
            {
                result.HataKodu = -1;
                result.Mesaj = "Marka eklenirken bir hata oluştu: " + ex.Message;
            }
            return result;
        }

        public TResult MarkaGuncelle(TblMarka marka)
        {
            var markaGuncelle = (from data in Context.TblMarka
                                 where data.MarkaId == marka.MarkaId
                                 select data).FirstOrDefault();
            if (markaGuncelle != null)
            {
                marka.MarkaAdi = marka.MarkaAdi;
                marka.MarkaSeo = marka.MarkaSeo;
                marka.GuncelleyenUyeId = marka.GuncelleyenUyeId;
                marka.GuncellemeTarihi = DateTime.Now;
                Context.SaveChanges();
                result.Basarili = true;
            }
            else
                result.Mesaj = "Marka güncellenirken bir hata oluştu: ";
            return result;
        }
    }
}
