using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Layer.Business.Interfaces.Ortak;
using Layer.Data;
using Layer.Entities;

namespace Layer.Business.Interfaces.Marka
{
    public interface IMarka : IDatabase, IIslemler
    {
        public TResult MarkaEkle(string markaAdi, string markaSeo, int ekleyenUyeId, DateTime eklenmeTarihi);
        public TResult MarkaEkle(TblMarka marka);
        public TResult MarkaGuncelle(string markaAdi, string markaSeo, int guncelleyenUyeId, DateTime guncellenmeTarihi);
        public TResult MarkaGuncelle(TblMarka marka);
        public TResult MarkaSil(int markaId);
        public TResult MarkaDetayGetir(int markaId);
        public TResult Markalar();
        public TResult MarkaListesi();
        public TResult MarkaFiltrele(int markaId, string markaAd);
    }
}
