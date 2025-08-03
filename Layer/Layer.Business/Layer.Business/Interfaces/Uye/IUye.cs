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
    public interface IUye : IDatabase, IIslemler, IUyeKisi
    {
        TResult KayitOl(int KisiId, int UyeId, string KisiAd, string KisiSoyad, long KisiTc, DateOnly KisiDogumTarihi, TCinsiyet Cinsiyet);
        TResult KayitOl(TblUye Uye, TblUyeKisi UyeKisi);
        TResult GirisYap(TblUye Uye);
        TResult UyelikSil(TblUyeKisi UyeKisi);
        TResult SifremiUnuttum(TblUyeKisi UyeKisi);
    }
}
