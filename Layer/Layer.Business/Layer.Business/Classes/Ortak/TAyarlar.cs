using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Layer.Business.Interfaces.Ortak;
using Layer.Data;
using Layer.Entities;

namespace Layer.Business.Classes.Ortak
{
    public class TAyarlar : IAyarlar
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

        public TResult AyarEkle(TblSiteAyarlar ayarlar)
        {
            try
            {
                Context.TblSiteAyarlar.Add(ayarlar);
                Context.SaveChanges();
                result.Basarili = true;
            }
            catch (Exception ex)
            {
                result.Mesaj = "Ayar eklenirken bir hata meydana geldi. Hata Kodu: -1";
            }
            return result;
        }

        public TResult AyarGuncelle(TblSiteAyarlar ayarlar)
        {
            try
            {
                var Ayarlar = (from data in Context.TblSiteAyarlar select data).FirstOrDefault();
                if (Ayarlar != null)
                {
                    Ayarlar = ayarlar;
                    Context.SaveChanges();
                    result.Basarili = true;
                }
            }
            catch (Exception ex)
            {
                result.Mesaj = "Ayar güncellenirken bir hata meydana geldi. Hata Kodu: -1";
            }
            return result;
        }

        public TResult AyarSil()
        {
            try
            {
                var Ayarlar = (from data in Context.TblSiteAyarlar select data).FirstOrDefault();
                if (Ayarlar != null)
                {
                    Context.TblSiteAyarlar.Remove(Ayarlar);
                    Context.SaveChanges();
                    result.Basarili = true;
                }
            }
            catch (Exception ex)
            {
                result.Mesaj = "Ayar silinirken bir hata meydana geldi. Hata Kodu: -1";
            }
            return result;
        }

        public TResult SiteAyarlari()
        {

            try
            {
                result.Veri = (from data in Context.TblSiteAyarlar select data).AsQueryable();
                if (result.Veri != null)
                    result.Basarili = true;
                else
                    result.Mesaj = "Site ayarları bulunamadı.";
            }
            catch (Exception ex)
            {
                result.Mesaj = "Site ayarları alınırken bir hata meydana geldi. Hata Kodu: -1";
            }
            return result;
        }
    }
}
