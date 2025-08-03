using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Layer.Business.Interfaces.Ortak;
using Layer.Data;
using Layer.Entities;

namespace Layer.Business.Interfaces.Sepet
{
    public interface ISepet : IIslemler, IDatabase
    {
        public TResult SepeteEkle(TblSepet sepet);
        public TResult SepetiSil(int sepetId);
        public TResult SepetiGuncelle(TblSepet sepet);
        public TResult SepetiListele(int uyeId);
    }
}
