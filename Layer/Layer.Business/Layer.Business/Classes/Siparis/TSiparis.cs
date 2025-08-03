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
    public class TSiparis : ISiparis
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

        public TResult SiparisleriListele(int uyeId)
        {
            var result = new TResult();

            try
            {
                var siparisler = Context.TblSiparis
                    .Where(s => s.UyeId == uyeId)
                    .OrderByDescending(s => s.SiparisTarihi)
                    .AsQueryable();


                result.Basarili = true;
                result.Mesaj = "Siparişler listelendi.";
                result.Veri = siparisler;
            }
            catch (Exception ex)
            {
                result.Basarili = false;
                result.Mesaj = "Hata: " + ex.Message;
            }

            return result;
        }

        public TResult SiparisOlustur(int uyeId, int adresId)
        {
            var result = new TResult();

            try
            {
                var sepetUrunler = Context.TblSepet
                    .Where(s => s.UyeId == uyeId)
                    .ToList();

                if (!sepetUrunler.Any())
                {
                    result.Basarili = false;
                    result.Mesaj = "Sepetinizde ürün bulunmamaktadır.";
                    return result;
                }

                var yeniSiparis = new TblSiparis
                {
                    UyeId = uyeId,
                    AdresId = adresId,
                    SiparisTarihi = DateTime.Now,
                    OdemeDurumu = true,
                    ToplamTutar = sepetUrunler.Sum(s => s.ToplamFiyat),
                };

                Context.TblSiparis.Add(yeniSiparis);
                Context.SaveChanges();

                foreach (var item in sepetUrunler)
                {
                    var urun = Context.TblUrun.FirstOrDefault(x => x.UrunId == item.UrunId);
                    if (urun != null)
                    {
                        urun.Stok -= item.Adet;
                        if (urun.Stok < 0) urun.Stok = 0;
                    }

                    var detay = new TblSiparisDetay
                    {
                        SiparisId = yeniSiparis.SiparisId,
                        UrunId = item.UrunId,
                        Adet = item.Adet,
                        UrunFiyat = urun.UrunFiyat,
                        ToplamTutar = item.ToplamFiyat
                    };

                    Context.TblSiparisDetay.Add(detay);
                }

                Context.TblSepet.RemoveRange(sepetUrunler);
                Context.SaveChanges();

                result.Basarili = true;
                result.Mesaj = "Sipariş başarıyla oluşturuldu.";
            }
            catch (Exception ex)
            {
                result.Basarili = false;
                result.Mesaj = "Hata oluştu: " + ex.Message;
            }

            return result;
        }

    }
}

