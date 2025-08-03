using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Layer.Business.Interfaces.Ortak;
using Layer.Data;
using Layer.Entities;

namespace Layer.Business.Interfaces.Favori
{
    public interface IFavori : IIslemler, IDatabase
    {
        public TResult FavoriEkle(TblFavori favori);
        public TResult FavoriSil(int favoriId);
        TResult FavoriListele(int uyeId);
    }
}
