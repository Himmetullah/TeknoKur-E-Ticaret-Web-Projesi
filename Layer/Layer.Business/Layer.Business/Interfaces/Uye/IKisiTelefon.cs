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
    public interface IKisiTelefon : IIslemler, IDatabase
    {
        public TResult KisiTelefonEkle(TblKisiTelefon KisiTelefon);
        public TResult KisiTelefonSil(int KisiTelefonId);
        public TResult KisiTelefonGuncelle(TblKisiTelefon KisiTelefon);
        public TResult KisiTelefonGetir();
        public TResult KisiTelefonDetayGetir(int KisiTelefonId, bool Silik);
    }
}
