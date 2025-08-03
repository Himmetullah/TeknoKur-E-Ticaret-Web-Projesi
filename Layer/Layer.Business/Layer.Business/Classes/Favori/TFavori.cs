using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Layer.Business.Interfaces.Favori;
using Layer.Business.Interfaces.Ortak;
using Layer.Data;
using Layer.Entities;

namespace Layer.Business.Classes.Favori
{
    public class TFavori : IFavori
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

        public TResult FavoriEkle(TblFavori favori)
        {
            try
            {
                var mevcutFavori = (from data in Context.TblFavori
                                       where data.UyeId == favori.UyeId && data.UrunId == favori.UrunId
                                       select data).FirstOrDefault();

                if (mevcutFavori != null)
                {
                    if (mevcutFavori.Silik == true)
                    {
                        mevcutFavori.Silik = false;
                        mevcutFavori.EklenmeTarihi = DateTime.Now;
                        Context.SaveChanges();
                        result.Basarili = true;
                        result.Mesaj = "Favori başarıyla geri yüklendi.";
                    }
                    else
                    {
                        result.Basarili = false;
                        result.Mesaj = "Bu ürün zaten favorilerinizde mevcut.";
                    }
                }
                else
                {
                    favori.EklenmeTarihi = DateTime.Now;
                    Context.TblFavori.Add(favori);
                    Context.SaveChanges();
                    result.Basarili = true;
                    result.Mesaj = "Favori başarıyla eklendi.";
                }
            }
            catch (Exception ex)
            {
                result.HataKodu = -1;
                result.Basarili = false;
                result.Mesaj = "Favori eklenirken bir hata oluştu.";
            }
            return result;
        }

        public TResult FavoriListele(int uyeId)
        {
            try
            {
                var favoriler = (from data in Context.TblFavori
                                 where data.UyeId == uyeId && data.Silik != true
                                 select data).AsQueryable();
                result.Veri = favoriler;
                result.Basarili = true;
                if (!favoriler.Any())
                {
                    result.Mesaj = "Favori listeniz boş.";
                }
                else
                {
                    result.Mesaj = "Favori listeniz başarıyla getirildi.";
                }
            }
            catch (Exception ex)
            {
                result.HataKodu = -1;
                result.Basarili = false;
                result.Mesaj = "Favori listesi alınırken bir hata oluştu.";
            }
            return result;
        }

        public TResult FavoriSil(int favoriId)
        {
            var favoriSil = (from data in Context.TblFavori
                             where data.FavoriId == favoriId
                             select data).FirstOrDefault();
            if (favoriSil != null)
            {
                favoriSil.Silik = true;
                Context.SaveChanges();
                result.Basarili = true;
            }
            else
                result.Mesaj = "Favori eklenemedi.";
            return result;
        }
    }
}
