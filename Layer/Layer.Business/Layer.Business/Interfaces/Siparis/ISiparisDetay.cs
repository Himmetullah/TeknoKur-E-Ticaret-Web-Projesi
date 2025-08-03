using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Layer.Business.Interfaces.Ortak;
using Layer.Data;
using Layer.Entities;

namespace Layer.Business.Interfaces.Siparis
{
    public interface ISiparisDetay : IDatabase, IIslemler
    {
        public TResult SiparisDetayGetir(int siparisId);
        public TResult SiparisDetayEkle(TblSiparisDetay detay);
        public TResult SiparisDetayGuncelle(TblSiparisDetay detay);
        public TResult SiparisDetaySil(int siparisDetayId);
    }
}
