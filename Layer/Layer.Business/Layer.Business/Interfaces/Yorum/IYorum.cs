using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Layer.Business.Interfaces.Ortak;
using Layer.Data;
using Layer.Entities;

namespace Layer.Business.Interfaces.Yorum
{
    public interface IYorum : IIslemler, IDatabase
    {
        TResult YorumEkle(TblYorum yorum);
        TResult YorumlariListele(int urunId);
        TResult OnayBekleyenYorumlariListele();
        TResult YorumOnayla(int yorumId);
        TResult YorumSil(int yorumId);
        TResult YorumDetayGetir(int yorumId);
    }
}
