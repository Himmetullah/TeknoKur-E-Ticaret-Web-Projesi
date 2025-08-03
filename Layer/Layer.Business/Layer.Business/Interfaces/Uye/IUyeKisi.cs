using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Layer.Business.Interfaces.Ortak;
using Layer.Data;
using Layer.Entities;
using Layer.Enum;

namespace Layer.Business.Interfaces.Uye
{
    public interface IUyeKisi : IDatabase, IIslemler
    {
        public TResult UyeKisiEkle(int KisiId, int UyeId, string KisiAd, string KisiSoyad, string KisiEmail, string KisiTelefon, string KisiAdres, DateTime KisiDogumTarihi, string KisiCinsiyet, string KisiTcKimlikNo, bool Silik = false);
        public TResult UyeKisiEkle(TblUyeKisi uyeKisi);
        public TResult UyeKisiSil(int KisiId, int UyeId);
        public TResult UyeKisiSil(int KisiId);
        public TResult UyeKisiSil(int UyeId, bool kullanilmicak);
        public TResult UyeKisiGuncelle(int KisiId, int UyeId, string KisiAd, string KisiSoyad, string KisiEmail, string KisiTelefon, string KisiAdres, DateTime KisiDogumTarihi, string KisiCinsiyet, string KisiTcKimlikNo);
        public TResult UyeKisiGuncelle(int KisiId, TblUyeKisi UyeKisi);
        public TResult UyeKisileriGetir();
        public TResult UyeKisiFiltrele(string Ad, string Soyad = "");
        public TResult UyeKisiFiltrele(TCinsiyet Cinsiyet);
        public TResult UyeKisiDetayGetir(int KisiId);
        public TResult UyeKisiDetayGetir(long KisiTc);
    }
}
