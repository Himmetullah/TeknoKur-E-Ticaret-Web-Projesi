using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Layer.Business.Interfaces.Ortak;
using Layer.Business.Interfaces.Uye;
using Layer.Data;
using Layer.Entities;

namespace Layer.Business.Classes.Uye
{
    public class TKisiAdres : IKisiAdres
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

        public TResult KisiAdresEkle(TblKisiAdres Adres)
        {
            try
            {
                Adres.Silik = false;
                Adres.EklenmeTarihi = DateTime.Now;
                Context.TblKisiAdres.Add(Adres);
                Context.SaveChanges();
                result.Basarili = true;
            }
            catch (Exception ex)
            {
                result.HataKodu = -1;
                result.Mesaj = "Adres eklenirken bir hata meydana geldi.";
            }
            return result;
        }

        public TResult KisiAdresSil(int AdresId)
        {
            var AdresSil = (from data in Context.TblKisiAdres
                            where data.AdresId == AdresId
                            select data).FirstOrDefault();
            if (AdresSil != null)
            {
                AdresSil.Silik = true;
                Context.SaveChanges();
                result.Basarili = true;
            }
            else
                result.Mesaj = "Adres silinirken bir hata meydana geldi.";
            return result;

        }

        public TResult KisiAdresGuncelle(TblKisiAdres Adres)
        {
            var AdresGuncelle = (from data in Context.TblKisiAdres
                                 where data.AdresId == Adres.AdresId
                                 select data).FirstOrDefault();
            if (AdresGuncelle != null)
            {
                AdresGuncelle.AdresBaslik = Adres.AdresBaslik;
                AdresGuncelle.AdresUlke = Adres.AdresUlke;
                AdresGuncelle.AdresIl = Adres.AdresIl;
                AdresGuncelle.AdresIlce = Adres.AdresIlce;
                AdresGuncelle.Adres = Adres.Adres;
                AdresGuncelle.Varsayilan = Adres.Varsayilan;
                AdresGuncelle.FaturaAdresi = Adres.FaturaAdresi;
                AdresGuncelle.GuncelleyenUyeId = Adres.GuncelleyenUyeId;
                AdresGuncelle.GuncellemeTarihi = DateTime.Now;
                Context.SaveChanges();
                result.Basarili = true;
            }
            else
                result.Mesaj = "Güncellenecek adres bulunamadı.";
            return result;
        }

        public TResult KisiAdresGetir()
        {
            result.Veri = Context.TblKisiAdres.AsQueryable();
            return result;
        }

        public TResult KisiAdresDetayGetir(int KisiId, bool Silik)
        {
            var AdresDetay = (from data in Context.TblKisiAdres
                              where data.KisiId == KisiId
                              orderby data.Silik != true
                              select data).FirstOrDefault();
            return result;
        }
    }
}
