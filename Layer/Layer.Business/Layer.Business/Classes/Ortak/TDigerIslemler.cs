using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Layer.Business.Classes.Ortak
{
    public static class TDigerIslemler
    {
        public static string SeoOlustur(string Metin)
        {
            string result = Metin.ToLower();
            result = result.Replace("ş", "s");
            result = result.Replace("ğ", "g");
            result = result.Replace("ı", "i");
            result = result.Replace("ç", "c");
            result = result.Replace("ö", "o");
            result = result.Replace("ü", "u");
            result = result.Replace(" ", "-");
            result = result.Replace("'", "");
            result = result.Replace("!", "");
            result = result.Replace("?", "");
            result = result.Replace(":", "");
            result = result.Replace(";", "");
            result = result.Replace(",", "");
            result = result.Replace(".", "");
            result = result.Replace("(", "");
            result = result.Replace(")", "");
            result = result.Replace("&", "");
            result = result.Replace("#", "");
            result = result.Replace("@", "");
            result = result.Replace("$", "");
            result = result.Replace("%", "");
            result = result.Replace("^", "");
            result = result.Replace("*", "");
            result = result.Replace("+", "");
            result = result.Replace("=", "");
            result = result.Replace("{", "");
            result = result.Replace("}", "");
            result = result.Replace("[", "");
            result = result.Replace("]", "");
            result = result.Replace("|", "");
            result = result.Replace("~", "");
            result = result.Replace("`", "");
            result = result.Replace("<", "");
            result = result.Replace(">", "");
            result = result.Replace("?", "");
            result = result.Replace(" ", "-");
            result = result.Replace("'", "");
            result = result.Replace("!", "");
            result = result.Replace(":", "");
            result = result.Replace(";", "");
            result = result.Replace(",", "");
            return result;
        }
    }
}
