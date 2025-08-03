using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Layer.Entities
{
    public class TResult
    {
        public bool Basarili { get; set; } = false;
        public string Mesaj { get; set; } = "";
        public DateTime HataZamani { get; set; } = DateTime.Now;
        public int HataKodu { get; set; } = 0;
        public List<object> Veriler { get; set; } = new List<object>();
        public IQueryable Veri { get; set; }
        public object Data { get; set; } = new object();
    }
}
