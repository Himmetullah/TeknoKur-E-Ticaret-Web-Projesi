using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Layer.Business.Interfaces.Ortak;
using Layer.Business.Interfaces.Yorum;
using Layer.Data;
using Layer.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Layer.Business.Classes.Yorum
{
    public class TYorum : IYorum
    {
        public DbTeknoKurEntities Context { get; set; } = new();

        public event IIslemler.RxEklemedenOnce EklemedenOnce;
        public event IIslemler.RxEklediktenSonra EklediktenSonra;
        public event IIslemler.RxSilmedenOnce SilmedenOnce;
        public event IIslemler.RxSildiktenSonra SildiktenSonra;
        public event IIslemler.RxGuncellemedenOnce GuncellemedenOnce;
        public event IIslemler.RxGuncellediktenSonra GuncellediktenSonra;
        public event IIslemler.RxFiltrelemedenOnce FiltrelemedenOnce;
        public event IIslemler.RxFiltrelediktenSonra FiltrelediktenSonra;
        public event IIslemler.RxGirisYapmadanOnce GirisYapmadanOnce;
        public event IIslemler.RxGirisYaptiktanSonra GirisYaptiktanSonra;
        public event IIslemler.RxKayitOlmadanOnce KayitOlmadanOnce;
        public event IIslemler.RxKayitOlduktanSonra KayitOlduktanSonra;

        public TResult OnayBekleyenYorumlariListele()
        {
            var result = new TResult();

            try
            {
                var yorumlar = Context.TblYorum
                    .Where(y => y.Onaylandi == false && y.Silik == false)
                    .AsQueryable();

                result.Veri = yorumlar;
                result.Basarili = true;
                result.Mesaj = "Onay bekleyen yorumlar başarıyla listelendi.";
            }
            catch (Exception)
            {
                result.Basarili = false;
                result.Mesaj = "Onay bekleyen yorumlar listelenirken bir hata oluştu!!";
            }
            return result;
        }

        public TResult YorumDetayGetir(int yorumId)
        {
            var result = new TResult();

            try
            {
                var yorum = Context.TblYorum.FirstOrDefault(y => y.YorumId == yorumId);
                if (yorum == null)
                {
                    result.Basarili = false;
                    result.Mesaj = "Yorum bulunamadı.";
                    return result;
                }

                result.Data = yorum; 
                result.Basarili = true;
                result.Mesaj = "Yorum detayları getirildi.";
            }
            catch (Exception)
            {
                result.Basarili= false;
                result.Mesaj = "Yorum detayları getirilirken bir hata oluştu!!";
            }
            return result;
        }

        public TResult YorumEkle(TblYorum yorum)
        {
            var result = new TResult();

            try
            {
                yorum.YorumTarihi = DateTime.Now;
                yorum.Onaylandi = false;
                yorum.Silik = false;

                Context.TblYorum.Add(yorum);
                Context.SaveChanges();

                result.Data = yorum;
                result.Basarili = true;
                result.Mesaj = "Yorumunuz başarıyla eklendi ve yönetici onayına gönderildi.";
            }
            catch (Exception)
            {
                result.Basarili = false;
                result.Mesaj = "Yorum eklenirken bir hata oluştu!!";
            }
            return result;
        }

        public TResult YorumlariListele(int urunId)
        {
            var result = new TResult();

            try
            {
                var yorumlar = Context.TblYorum
                    .Where(y => y.UrunId == urunId && y.Onaylandi == true &&
                    y.Silik == false).AsQueryable();

                result.Veri = yorumlar;
                result.Basarili = true;
                result.Mesaj = "Yorumlar başarıyla listelendi.";
            }
            catch (Exception)
            {
                result.Basarili = false;
                result.Mesaj = "Yorumlar listelenirken bir hata oluştu!!";
            }
            return result;
        }
        public TResult OnaylanmisYorumlariListele()
        {
            var result = new TResult();

            try
            {
                var yorumlar = Context.TblYorum
                    .Where(y => y.Onaylandi == true && y.Silik == false)
                    .OrderByDescending(y => y.YorumTarihi)
                    .AsQueryable();

                result.Veri = yorumlar;
                result.Basarili = true;
                result.Mesaj = "Onaylanmış yorumlar başarıyla listelendi.";
            }
            catch (Exception)
            {
                result.Basarili = false;
                result.Mesaj = "Onaylanmış yorumlar listelenirken bir hata oluştu!!";
            }
            return result;
        }

        public TResult YorumOnayla(int yorumId)
        {
            var result = new TResult();
            try
            {
                var yorum = Context.TblYorum.FirstOrDefault(y => y.YorumId == yorumId);
                if (yorum == null)
                {
                    result.Basarili = false;
                    result.Mesaj = "Yorum bulunamadı.";
                    return result;
                }

                yorum.Onaylandi = true;
                Context.SaveChanges();

                result.Data = yorum; 
                result.Basarili = true;
                result.Mesaj = "Yorum başarıyla onaylandı.";
            }
            catch (Exception)
            {
                result.Basarili = false;
                result.Mesaj = "Yorum onaylanırken bir hata oluştu!!";
            }
            return result;

        }

        public TResult YorumSil(int yorumId)
        {
            var result = new TResult();
            try
            {
                var yorum = Context.TblYorum.FirstOrDefault(y => y.YorumId == yorumId);
                if (yorum == null)
                {
                    result.Basarili = false;
                    result.Mesaj = "Yorum bulunamadı!!";
                    return result;
                }

                yorum.Silik = true;
                Context.SaveChanges();

                result.Data = yorum; 
                result.Basarili = true;
                result.Mesaj = "Yorum başarıyla silindi.";
            }
            catch (Exception)
            {
                result.Basarili = false;
                result.Mesaj = "Yorum silinirken bir hata oluştu!!";
            }
            return result;
        }
    }
}

