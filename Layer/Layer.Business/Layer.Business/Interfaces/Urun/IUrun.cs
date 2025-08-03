using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Layer.Business.Interfaces.Ortak;
using Layer.Data;
using Layer.Entities;

namespace Layer.Business.Interfaces.Urun
{
    public interface IUrun : IIslemler, IDatabase
    {
        public TResult UrunEkle(string urunAdi, string urunAciklama, string urunResimUrl ,decimal urunFiyati, 
            string urunKodu, int kategoriId, int markaId, string urunSeo, string urunTaglar, string urunTeknikOzellikler, 
            int urunStok, int EkleyenUyeId, DateTime EklenmeTarihi);
        public TResult UrunGuncelle(string urunAdi, string urunAciklama, string urunResimUrl, decimal urunFiyati,
            string urunKodu, int kategoriId, int markaId, string urunSeo, string urunTaglar, string urunTeknikOzellikler,
            int urunStok, int GuncelleyenUyeId, DateTime GuncellenmeTarihi);

        public TResult UrunEkle(TblUrun urun);
        public TResult UrunGuncelle(TblUrun urun);      
        public TResult UrunSil(int urunId);
        public TResult UrunDetayGetir(int urunId);
        public TResult Urunler();
        public TResult UrunListesi();
        public TResult UrunFiltrele(int urunId, string urunAd, string urunKodu);
    }
}
