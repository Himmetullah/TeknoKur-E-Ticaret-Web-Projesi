namespace Api.Customer.Models
{
    public class Urun
    {
        public int UrunId { get; set; }
        public int KategoriId { get; set; }
        public int MarkaId { get; set; }
        public string UrunAdi { get; set; }
        public string UrunKodu { get; set; }
        public string UrunResimUrl { get; set; }
        public string UrunSeo { get; set; }
        public string UrunTaglar { get; set; }
        public decimal UrunFiyat { get; set; }
        public string UrunAciklama { get; set; }
        public string TeknikOzellikler { get; set; }
        public int Stok { get; set; }

        public int? EkleyenUyeId { get; set; }
        public DateTime? EklenmeTarihi { get; set; }
        public bool? Silik { get; set; }
        public int? SilenUyeId { get; set; }
        public DateTime? SilmeTarihi { get; set; }
        public int? GuncelleyenUyeId { get; set; }
        public DateTime? GuncellemeTarihi { get; set; }
    }
}
