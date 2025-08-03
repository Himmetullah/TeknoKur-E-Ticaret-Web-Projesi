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
    public class TKisiTelefon : IKisiTelefon
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

        public TResult KisiTelefonDetayGetir(int KisiTelefonId, bool Silik)
        {
            var Detay = (from data in Context.TblKisiTelefon
                         where data.TelefonId == KisiTelefonId &&
                         data.Silik != true
                         select data).FirstOrDefault();
            return result;
        }

        public TResult KisiTelefonEkle(TblKisiTelefon KisiTelefon)
        {
            try
            {
                KisiTelefon.Silik = false;
                KisiTelefon.EklenmeTarihi = DateTime.Now;
                Context.TblKisiTelefon.Add(KisiTelefon);
                Context.SaveChanges();
                result.Basarili = true;
            }
            catch (Exception ex)
            {
                result.HataKodu = -1;
                result.Mesaj = "Telefon eklenirken bir hata meydana geldi.";
            }
            return result;
        }

        public TResult KisiTelefonGetir()
        {
            result.Veri = Context.TblKisiTelefon.AsQueryable();
            return result;
        }

        public TResult KisiTelefonGuncelle(TblKisiTelefon KisiTelefon)
        {
            var TelefonGuncelle = (from data in Context.TblKisiTelefon
                                   where data.TelefonId == KisiTelefon.TelefonId
                                   select data).FirstOrDefault();
            if (TelefonGuncelle != null)
            {
                TelefonGuncelle.TelefonNumarasi = KisiTelefon.TelefonNumarasi;
                TelefonGuncelle.Varsayilan = KisiTelefon.Varsayilan;
                TelefonGuncelle.GuncelleyenUyeId = KisiTelefon.GuncelleyenUyeId;
                TelefonGuncelle.GuncellemeTarihi = DateTime.Now;
                Context.SaveChanges();
                result.Basarili = true;
            }
            else
                result.Mesaj = "Güncellenecek telefon bulunamadı.";
            return result;
        }

        public TResult KisiTelefonSil(int KisiTelefonId)
        {
            var TelefonSil = (from data in Context.TblKisiTelefon
                              where data.TelefonId == KisiTelefonId
                              select data).FirstOrDefault();
            if (TelefonSil != null)
            {
                TelefonSil.Silik = true;
                Context.SaveChanges();
                result.Basarili = true;
            }
            else
                result.Mesaj = "Silinecek telefon bulunamadı.";
            return result;
        }
    }
}
