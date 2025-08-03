using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Layer.Security
{
    public class TSecurity
    {
        private const string Rakamlar = "0123456789";
        private const string OzelKarakter = "!@#$%^&*()_+-=[]{}|;:',.<>?";
        private const string BuyukHarfler = "ABCDEFGHIJKLMNOPQRSTUVWXYZWXQÖĞÇ";

        public string MetindenSifrele(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes)
                    sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
        }
        public bool SifreUzunlukKontrol(string Sifre, int ZorunluUzunluk)
        {
            return (Sifre.Length >= ZorunluUzunluk ? true : false);
        }
        public bool SifreKontrol(string Sifre)
        {
            bool RakamVarMi = false;
            bool OzelKarakterVarMi = false;
            bool BuyukHarfVarMi = false;

            foreach (var Siradaki in Sifre)
            {
                RakamVarMi = EnAzBirRakamVarMi(Siradaki.ToString());
                if (RakamVarMi)
                    break;
            }
            foreach (var Siradaki in Sifre)
            {
                OzelKarakterVarMi = EnAzBirOzelKarakterVarMi(Siradaki.ToString());
                if (OzelKarakterVarMi)
                    break;
            }
            foreach (var Siradaki in Sifre)
            {
                BuyukHarfVarMi = EnAzBirBuyukHarfVarMi(Siradaki.ToString());
                if (BuyukHarfVarMi)
                    break;
            }
            return (RakamVarMi && OzelKarakterVarMi && BuyukHarfVarMi ? true : false);

        }
        private bool EnAzBirRakamVarMi(string Karakter)
        {
            return (Rakamlar.IndexOf(Karakter) > -1 ? true : false);
        }
        private bool EnAzBirOzelKarakterVarMi(string Karakter)
        {
            return (OzelKarakter.IndexOf(Karakter) > -1 ? true : false);
        }
        private bool EnAzBirBuyukHarfVarMi(string Karakter)
        {
            return (BuyukHarfler.IndexOf(Karakter) > -1 ? true : false);
        }
    }
}
