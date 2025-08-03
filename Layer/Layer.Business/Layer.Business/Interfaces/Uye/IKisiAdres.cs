using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Layer.Business.Interfaces.Ortak;
using Layer.Data;
using Layer.Entities;

namespace Layer.Business.Interfaces.Uye
{
    public interface IKisiAdres : IIslemler, IDatabase
    {
        public TResult KisiAdresEkle(TblKisiAdres Adres);
        public TResult KisiAdresSil(int AdresId);
        public TResult KisiAdresGuncelle(TblKisiAdres Adres);
        public TResult KisiAdresGetir();
        public TResult KisiAdresDetayGetir(int KisiId, bool Silik);

    }
}
