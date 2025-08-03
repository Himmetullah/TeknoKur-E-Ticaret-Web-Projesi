using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Layer.Data;
using Layer.Entities;

namespace Layer.Business.Interfaces.Ortak
{
    public interface IAyarlar : IIslemler, IDatabase
    {
        public TResult SiteAyarlari();
        public TResult AyarEkle(TblSiteAyarlar ayarlar);
        public TResult AyarGuncelle(TblSiteAyarlar ayarlar);
        public TResult AyarSil();
    }
}
