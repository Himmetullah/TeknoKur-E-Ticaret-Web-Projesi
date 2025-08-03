using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Layer.Business.Interfaces.Ortak;
using Layer.Entities;

namespace Layer.Business.Interfaces.Siparis
{
    public interface ISiparis : IDatabase, IIslemler
    {
        public TResult SiparisOlustur(int uyeId, int adresId);
        public TResult SiparisleriListele(int uyeId);
    }
}
