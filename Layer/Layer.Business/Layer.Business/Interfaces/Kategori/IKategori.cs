using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Layer.Business.Interfaces.Ortak;
using Layer.Data;
using Layer.Entities;

namespace Layer.Business.Interfaces.Kategori
{
    public interface IKategori : IDatabase, IIslemler
    {
        public TResult KategoriEkle(string kategoriAdi, string kategoriSeo, int? ustKategoriId, int ekleyenUyeId, DateTime eklenmeTarihi);
        public TResult KategoriEkle(TblKategoriler kategori);
        public TResult KategoriGuncelle(string kategoriAdi, string kategoriSeo, int? ustKategoriId, int guncelleyenUyeId, DateTime guncellenmeTarihi);
        public TResult KategoriGuncelle(TblKategoriler kategori);
        public TResult KategoriSil(int kategoriId);
        public TResult KategoriDetayGetir(int kategoriId);
        public TResult Kategoriler();
        public TResult KategoriListesi();
        public TResult KategoriFiltrele(int kategoriId, string kategoriAd);
    }
}
