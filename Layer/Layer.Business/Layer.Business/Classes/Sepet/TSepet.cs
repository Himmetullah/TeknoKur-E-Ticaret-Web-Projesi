using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Layer.Business.Interfaces.Ortak;
using Layer.Business.Interfaces.Sepet;
using Layer.Data;
using Layer.Entities;

namespace Layer.Business.Classes.Sepet
{
    public class TSepet : ISepet
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

        public TResult SepeteEkle(TblSepet sepet)
        {
            var result = new TResult(); // <-- DİKKAT: her seferinde yeni!
            try
            {
                var urun = Context.TblUrun.FirstOrDefault(x => x.UrunId == sepet.UrunId);
                if (urun == null)
                {
                    result.Basarili = false;
                    result.Mesaj = "Ürün bulunamadı veya pasif durumda.";
                    return result;
                }

                var mevcutSepetItem = Context.TblSepet.FirstOrDefault(x =>
                    x.UyeId == sepet.UyeId && x.UrunId == sepet.UrunId);

                if (mevcutSepetItem != null)
                {
                    int yeniAdet = mevcutSepetItem.Adet + sepet.Adet;
                    if (urun.Stok < yeniAdet)
                    {
                        result.Basarili = false;
                        result.Mesaj = $"Yeterli stok yok. Maksimum {urun.Stok} adet ekleyebilirsiniz.";
                        return result;
                    }
                    mevcutSepetItem.Adet = yeniAdet;
                    mevcutSepetItem.ToplamFiyat = yeniAdet * urun.UrunFiyat;
                }
                else
                {
                    if (urun.Stok < sepet.Adet)
                    {
                        result.Basarili = false;
                        result.Mesaj = $"Yeterli stok yok. Maksimum {urun.Stok} adet ekleyebilirsiniz.";
                        return result;
                    }
                    sepet.EklenmeTarihi = DateTime.Now;
                    sepet.ToplamFiyat = sepet.Adet * urun.UrunFiyat;
                    Context.TblSepet.Add(sepet);
                }
                Context.SaveChanges();
                result.Basarili = true;
                result.Mesaj = "Ürün başarıyla sepete eklendi.";
            }
            catch (Exception ex)
            {
                result.Basarili = false;
                result.Mesaj = "Hata: " + ex.Message;
            }
            return result;
        }

        public TResult SepetiListele(int uyeId)
        {
            var result = new TResult(); // <-- DİKKAT: her seferinde yeni!
            try
            {
                var sepet = Context.TblSepet.Where(x => x.UyeId == uyeId);
                result.Basarili = true;
                result.Veri = sepet;
                result.Mesaj = "Sepet listelendi.";
            }
            catch (Exception ex)
            {
                result.Basarili = false;
                result.Mesaj = "Sepet listelenirken hata: " + ex.Message;
            }
            return result;
        }

        public TResult SepetiSil(int sepetId)
        {
            var result = new TResult();
            var sil = Context.TblSepet.FirstOrDefault(x => x.SepetId == sepetId);
            if (sil != null)
            {
                Context.TblSepet.Remove(sil);
                Context.SaveChanges();
                result.Basarili = true;
                result.Mesaj = "Ürün sepetten kaldırıldı.";
            }
            else
            {
                result.Basarili = false;
                result.Mesaj = "Sepet silinemedi, ürün bulunamadı.";
            }
            return result;
        }

        public TResult SepetiGuncelle(TblSepet sepet)
        {
            var result = new TResult();
            var sepetGuncelle = Context.TblSepet.FirstOrDefault(x => x.SepetId == sepet.SepetId);
            if (sepetGuncelle != null)
            {
                var urun = Context.TblUrun.FirstOrDefault(x => x.UrunId == sepetGuncelle.UrunId && x.Silik != true);
                if (urun == null)
                {
                    result.Basarili = false;
                    result.Mesaj = "Ürün bilgisi bulunamadı.";
                    return result;
                }
                if (urun.Stok < sepet.Adet)
                {
                    result.Basarili = false;
                    result.Mesaj = $"Yeterli stok yok. Maksimum {urun.Stok} adet ekleyebilirsiniz.";
                    return result;
                }
                sepetGuncelle.Adet = sepet.Adet;
                sepetGuncelle.ToplamFiyat = sepet.Adet * urun.UrunFiyat;
                sepetGuncelle.EklenmeTarihi = DateTime.Now;
                Context.SaveChanges();
                result.Basarili = true;
                result.Mesaj = "Sepet güncellendi.";
            }
            else
            {
                result.Basarili = false;
                result.Mesaj = "Sepet güncellenemedi, ürün bulunamadı.";
            }
            return result;
        }
        public decimal GetSepetToplam(int uyeId)
        {
            var sepetUrun = Context.TblSepet
                     .Where(x => x.UyeId == uyeId)
                     .ToList();

            if (!sepetUrun.Any())
            {
                return 0;
            }

            var toplam = sepetUrun.Join(Context.TblUrun,
                sepetItem => sepetItem.UrunId,
                urun => urun.UrunId,
                (sepetItem, urun) => urun.UrunFiyat * sepetItem.Adet)
                .Sum();

            return toplam;
        }
    }
}
