using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Layer.Business.Interfaces.Ortak;
using Layer.Business.Interfaces.Uye;
using Layer.Data;
using Layer.Entities;
using Layer.Enum;
using Layer.Security;

namespace Layer.Business.Classes.Uye
{
    public class TUye : IUye
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
        private TResult result = new();

        public TResult GirisYap(TblUye Uye)
        {
            bool Izin = true;

            if (GirisYapmadanOnce != null)
                Izin = GirisYapmadanOnce(Uye.UyeId);

            if (!Izin)
            {
                result.Mesaj = "Giriş yapılamadı.";
                return result;  
            }

            try
            {
                TSecurity security = new();
                string SifreliSifre = security.MetindenSifrele(Uye.Sifre);

                var uye = Context.TblUye.Include(x => x.TblUyeKisi)
                          .FirstOrDefault(x => x.UyeMailAdresi == Uye.UyeMailAdresi &&
                          x.Sifre == SifreliSifre && x.Silik == false);

               

                if (uye != null)
                {
                    var kisi = uye.TblUyeKisi.FirstOrDefault();
                    result.Basarili = true;
                    result.Data = new
                    {
                        UyeId = uye.UyeId,
                        KisiAd = kisi?.KisiAd ?? "",
                        KisiSoyad = kisi?.KisiSoyad ?? "",
                        Donuk = uye.Donuk,
                        DondurmaTarihi = uye.DondurmaTarihi
                    };
                }
                else
                {
                    result.Basarili = false;
                    result.Mesaj = "Kullanıcı adı veya şifre hatalı.";
                }

                if (GirisYaptiktanSonra != null)
                    GirisYaptiktanSonra(uye, result);
            }
            catch (Exception ex)
            {
                result.Basarili = false;
                result.HataKodu = -1;
                result.Mesaj = "Bir hata oluştu.";
            }
            return result;
        }

        public TResult KayitOl(int KisiId, int UyeId, string KisiAd, string KisiSoyad, long KisiTc, DateOnly KisiDogumTarihi, TCinsiyet Cinsiyet)
        {
            throw new NotImplementedException();
        }

        public TResult KayitOl(TblUye Uye, TblUyeKisi UyeKisi)
        {
            bool Izin = false;
            if (KayitOlmadanOnce != null)
                Izin = KayitOlmadanOnce(Uye.UyeId);
            var MailKayitliMi = MailKontrol(Uye.UyeMailAdresi);

            if (MailKayitliMi)
            {
                result.HataKodu = 409;
                result.Mesaj = "Bu e-posta adresi zaten kayıtlı.";
            }
            else
            {
                TSecurity security = new();
                bool SifreKontrol = security.SifreKontrol(Uye.Sifre);
                bool SifreUzunMu = security.SifreUzunlukKontrol(Uye.Sifre, 8);
                if (!SifreUzunMu)
                    result.Mesaj = "Şifreniz yeterli uzunlukta değil. Lütfen en az 8 karakter olsun.";
                else if (!SifreKontrol)
                    result.Mesaj = "Şifreniz en az 1 Büyük Harf, 1 Özel Karakter ve 1 Sayı içermelidir !!";
                else
                {
                    string SifreliSifre = security.MetindenSifrele(Uye.Sifre);
                    Uye.Sifre = SifreliSifre;
                    Uye.Silik = false;
                    Uye.UyeKayitTarihi = DateTime.Now;
                    Uye.Donuk = false;
                    Context.TblUye.Add(Uye);
                    Context.SaveChanges();

                    UyeKisi.UyeId = Uye.UyeId;
                    Context.TblUyeKisi.Add(UyeKisi);
                    Context.SaveChanges();

                    result.Basarili = true;
                    result.Mesaj = "Kayıt başarılı.";
                }
            }
            return result;
        }

        public TResult SifremiUnuttum(TblUyeKisi UyeKisi)
        {
            throw new NotImplementedException();
        }

        public TResult UyeKisiDetayGetir(int KisiId)
        {
            var uyeKisi = (from data in Context.TblUyeKisi
                           where data.KisiId == KisiId
                           select data).AsQueryable();
            if (uyeKisi != null)
            {
                result.Basarili = true;
                result.Veri = uyeKisi;
            }
            else
                result.Mesaj = "Kişiler Getirilirken Bir Hata Oluştu.";
            return result;
        }

        public TResult UyeKisiDetayGetir(long KisiTc)
        {
            var uyeKisi = (from data in Context.TblUyeKisi
                           where data.KisiTc == KisiTc
                           select data).AsQueryable();
            if (uyeKisi != null)
            {
                result.Basarili = true;
                result.Veri = uyeKisi;
            }
            else
                result.Mesaj = "Kişiler Getirilirken Bir Hata Oluştu.";
            return result;
        }

        public TResult UyeKisiEkle(int KisiId, int UyeId, string KisiAd, string KisiSoyad, string KisiEmail, string KisiTelefon, string KisiAdres, DateTime KisiDogumTarihi, string KisiCinsiyet, string KisiTcKimlikNo, bool Silik = false)
        {
            try
            {
                Context.TblUyeKisi.Add(new TblUyeKisi
                {
                    KisiId = KisiId,
                    UyeId = UyeId,
                    KisiAd = KisiAd,
                    KisiSoyad = KisiSoyad,
                    KisiDogumTarihi = KisiDogumTarihi,
                    KisiCinsiyet = KisiCinsiyet,
                    KisiTc = long.Parse(KisiTcKimlikNo),
                    Silik = Silik
                });
                Context.SaveChanges();
                result.Basarili = true;
            }
            catch (Exception ex)
            {
                result.HataKodu = -1;
                result.Mesaj = "Kişi eklenirken bir hata oluştu: " + ex.Message;
            }
            return result;
        }

        public TResult UyeKisiEkle(TblUyeKisi uyeKisi)
        {
            try
            {
                Context.TblUyeKisi.Add(new TblUyeKisi
                {
                    KisiId = uyeKisi.KisiId,
                    UyeId = uyeKisi.UyeId,
                    KisiAd = uyeKisi.KisiAd,
                    KisiSoyad = uyeKisi.KisiSoyad,
                    KisiDogumTarihi = uyeKisi.KisiDogumTarihi,
                    KisiCinsiyet = uyeKisi.KisiCinsiyet,
                    KisiTc = uyeKisi.KisiTc,
                    Silik = uyeKisi.Silik,
                });
                Context.SaveChanges();
                result.Basarili = true;
            }
            catch (Exception ex)
            {
                result.HataKodu = -1;
                result.Mesaj = "Kişi eklenirken bir hata oluştu: " + ex.Message;
            }
            return result;
        }

        public TResult UyeKisiFiltrele(string Ad, string Soyad = "")
        {
            var uyeKisiFiltrele = (from data in Context.TblUyeKisi
                                   where data.KisiAd == Ad && data.KisiSoyad == Soyad
                                   select data).AsQueryable();
            result.Veri = uyeKisiFiltrele;
            return result;
        }

        public TResult UyeKisiFiltrele(TCinsiyet Cinsiyet)
        {
            var uyeKisiFiltrele = (from data in Context.TblUyeKisi
                                   where data.KisiCinsiyet == Cinsiyet.ToString()
                                   select data).AsQueryable();
            result.Veri = uyeKisiFiltrele;
            return result;
        }

        public TResult UyeKisiGuncelle(int KisiId, int UyeId, string KisiAd, string KisiSoyad, string KisiEmail, string KisiTelefon, string KisiAdres, DateTime KisiDogumTarihi, string KisiCinsiyet, string KisiTcKimlikNo)
        {
            var uyeKisiGuncelle = (from data in Context.TblUyeKisi
                                   where data.KisiId == KisiId
                                   select data).FirstOrDefault();
            if (uyeKisiGuncelle != null)
            {
                KisiId = uyeKisiGuncelle.KisiId;
                UyeId = uyeKisiGuncelle.UyeId;
                KisiAd = uyeKisiGuncelle.KisiAd;
                KisiSoyad = uyeKisiGuncelle.KisiSoyad;
                KisiDogumTarihi = uyeKisiGuncelle.KisiDogumTarihi ?? DateTime.Now;
                KisiCinsiyet = uyeKisiGuncelle.KisiCinsiyet;
                KisiTcKimlikNo = uyeKisiGuncelle.KisiTc.ToString();
                Context.SaveChanges();
                result.Basarili = true;
            }
            else
                result.Mesaj = "Kişi bulunamadı.";
            return result;
        }

        public TResult UyeKisiGuncelle(int KisiId, TblUyeKisi UyeKisi)
        {
            var uyeKisiGuncelle = (from data in Context.TblUyeKisi
                                   where data.KisiId == KisiId
                                   select data).FirstOrDefault();
            if (uyeKisiGuncelle != null)
            {
               UyeKisi.KisiId = uyeKisiGuncelle.KisiId;
               UyeKisi.UyeId = uyeKisiGuncelle.UyeId;
               UyeKisi.KisiAd = uyeKisiGuncelle.KisiAd;
               UyeKisi.KisiSoyad = uyeKisiGuncelle.KisiSoyad;
               UyeKisi.KisiDogumTarihi = uyeKisiGuncelle.KisiDogumTarihi ?? DateTime.Now;
               UyeKisi.KisiCinsiyet = uyeKisiGuncelle.KisiCinsiyet;
               UyeKisi.KisiTc = uyeKisiGuncelle.KisiTc;
               Context.SaveChanges();
               result.Basarili = true;
            }
            else
                result.Mesaj = "Kişi bulunamadı.";
            return result;
        }

        public TResult UyeKisileriGetir()
        {
            result.Veri = (from data in Context.TblUyeKisi
                           where data.Silik != true
                           select data).AsQueryable();
            return result;
        }

        public TResult UyeKisiSil(int KisiId, int UyeId)
        {
            var uyeKisiSil = (from data in Context.TblUyeKisi
                               where data.KisiId == KisiId && data.UyeId == UyeId
                               select data).FirstOrDefault();
            if (uyeKisiSil != null)
            {
                uyeKisiSil.Silik = true;
                Context.SaveChanges();
                result.Basarili = true;
                result.Mesaj = "Kişi silindi.";
            }
            else
                result.Mesaj = "Kişi silinirken bir hata oluştu.";
            return result;
        }

        public TResult UyeKisiSil(int KisiId)
        {
            var uyeKisiSil = (from data in Context.TblUyeKisi
                               where data.KisiId == KisiId
                               select data).FirstOrDefault();
            if (uyeKisiSil != null)
            {
                uyeKisiSil.Silik = true;
                Context.SaveChanges();
                result.Basarili = true;
                result.Mesaj = "Kişi silindi.";
            }
            else
                result.Mesaj = "Kişi silinirken bir hata oluştu.";
            return result;
        }

        public TResult UyeKisiSil(int UyeId, bool kullanilmicak)
        {
            var uyeKisiSil = (from data in Context.TblUyeKisi
                               where data.UyeId == UyeId
                               select data).FirstOrDefault();
            if (uyeKisiSil != null)
            {
                uyeKisiSil.Silik = true;
                Context.SaveChanges();
                result.Basarili = true;
                result.Mesaj = "Kişi silindi.";
            }
            else
                result.Mesaj = "Kişi silinirken bir hata oluştu.";
            return result;
        }

        public TResult UyelikSil(TblUyeKisi UyeKisi)
        {
            var uye = (from data in Context.TblUye
                      where data.UyeId == UyeKisi.UyeId
                      select data).FirstOrDefault();
            if (uye != null)
            {
                uye.Silik = true;
                uye.SilinmeTarihi = DateTime.Now;
                uye.Donuk = true;
                uye.DondurmaTarihi = DateTime.Now;
                Context.SaveChanges();
                result.Basarili = true;
            }
            else
                result.Mesaj = "Üyelik silinirken bir hata oluştu.";
            return result;
        }
        private bool MailKontrol(string Mail)
        {
            return (from data in Context.TblUye
                    where data.UyeMailAdresi == Mail
                    select data).Any();
        }
    }
}
