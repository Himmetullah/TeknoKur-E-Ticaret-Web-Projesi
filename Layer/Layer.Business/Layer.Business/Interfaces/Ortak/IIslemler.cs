using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Layer.Entities;

namespace Layer.Business.Interfaces.Ortak
{
    public interface IIslemler
    {
        delegate bool RxEklemedenOnce(object Tablo);
        delegate void RxEklediktenSonra(object Tablo, TResult Result);
        event RxEklemedenOnce EklemedenOnce;
        event RxEklediktenSonra EklediktenSonra;

        delegate bool RxSilmedenOnce(object Tablo);
        delegate void RxSildiktenSonra(object Tablo, TResult Result);
        event RxSilmedenOnce SilmedenOnce;
        event RxSildiktenSonra SildiktenSonra;

        delegate bool RxGuncellemedenOnce(object Tablo);
        delegate void RxGuncellediktenSonra(object Tablo, TResult Result);
        event RxGuncellemedenOnce GuncellemedenOnce;
        event RxGuncellediktenSonra GuncellediktenSonra;

        delegate bool RxFiltrelemedenOnce(object Tablo);
        delegate void RxFiltrelediktenSonra(object Tablo, TResult Result);
        event RxFiltrelemedenOnce FiltrelemedenOnce;
        event RxFiltrelediktenSonra FiltrelediktenSonra;

        delegate bool RxGirisYapmadanOnce(object Tablo);
        delegate void RxGirisYaptiktanSonra(object Tablo, TResult Result);
        event RxGirisYapmadanOnce GirisYapmadanOnce;
        event RxGirisYaptiktanSonra GirisYaptiktanSonra;

        delegate bool RxKayitOlmadanOnce(object Tablo);
        delegate void RxKayitOlduktanSonra(object Tablo, TResult Result);
        event RxKayitOlmadanOnce KayitOlmadanOnce;
        event RxKayitOlduktanSonra KayitOlduktanSonra;
    }
}
