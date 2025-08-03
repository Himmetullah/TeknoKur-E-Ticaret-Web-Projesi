using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Layer.Business.Interfaces.Ortak;
using Layer.Business.Interfaces.Siparis;
using Layer.Data;
using Layer.Entities;

namespace Layer.Business.Classes.Siparis
{
    internal class TSiparisDetay : ISiparisDetay
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

        public TResult SiparisDetayEkle(TblSiparisDetay detay)
        {
            var result = new TResult();
            try
            {
                Context.TblSiparisDetay.Add(detay);
                Context.SaveChanges();

                result.Basarili = true;
                result.Mesaj = "Sipariş detayı başarıyla eklendi.";
            }
            catch (Exception ex)
            {
                result.Basarili = false;
                result.Mesaj = "Hata: " + ex.Message;
            }
            return result;
        }

        public TResult SiparisDetayGetir(int siparisId)
        {
            var result = new TResult();
            try
            {
                var detaylar = Context.TblSiparisDetay
                    .Where(d => d.SiparisId == siparisId)
                    .ToList();

                result.Basarili = true;
                result.Mesaj = "Sipariş detayları getirildi.";
                result.Veri = detaylar.AsQueryable();
            }
            catch (Exception ex)
            {
                result.Basarili = false;
                result.Mesaj = "Hata: " + ex.Message;
            }
            return result;
        }

        public TResult SiparisDetayGuncelle(TblSiparisDetay detay)
        {
            var result = new TResult();
            try
            {
                var eskiDetay = Context.TblSiparisDetay.Find(detay.SiparisDetayId);
                if (eskiDetay != null)
                {
                    eskiDetay.Adet = detay.Adet;
                    eskiDetay.UrunFiyat = detay.UrunFiyat;
                    eskiDetay.ToplamTutar = detay.ToplamTutar;
                    Context.SaveChanges();

                    result.Basarili = true;
                    result.Mesaj = "Sipariş detayı güncellendi.";
                }
                else
                {
                    result.Basarili = false;
                    result.Mesaj = "Sipariş detayı bulunamadı.";
                }
            }
            catch (Exception ex)
            {
                result.Basarili = false;
                result.Mesaj = "Hata: " + ex.Message;
            }
            return result;
        }

        public TResult SiparisDetaySil(int siparisDetayId)
        {
            var result = new TResult();
            try
            {
                var detay = Context.TblSiparisDetay.Find(siparisDetayId);
                if (detay != null)
                {
                    Context.TblSiparisDetay.Remove(detay);
                    Context.SaveChanges();

                    result.Basarili = true;
                    result.Mesaj = "Sipariş detayı silindi.";
                }
                else
                {
                    result.Basarili = false;
                    result.Mesaj = "Sipariş detayı bulunamadı.";
                }
            }
            catch (Exception ex)
            {
                result.Basarili = false;
                result.Mesaj = "Hata: " + ex.Message;
            }
            return result;
        }
    }
}
