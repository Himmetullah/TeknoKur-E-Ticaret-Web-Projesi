using Api.Customer.Data;
using Api.Customer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Customer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UrunController : Controller
    {
        private readonly AppDbContext _context;

        public UrunController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var urunler = await _context.TblUrun.ToListAsync();
            return Ok(urunler);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var urun = await _context.TblUrun.FindAsync(id);
            if (urun == null) return NotFound();
            return Ok(urun);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Urun urun)
        {
            urun.EklenmeTarihi = DateTime.Now;
            _context.TblUrun.Add(urun);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = urun.UrunId }, urun);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Urun urun)
        {
            if (id != urun.UrunId) return BadRequest();

            var mevcutUrun = await _context.TblUrun.FindAsync(id);
            if (mevcutUrun == null) return NotFound();

            mevcutUrun.UrunAdi = urun.UrunAdi;
            mevcutUrun.UrunKodu = urun.UrunKodu;
            mevcutUrun.UrunResimUrl = urun.UrunResimUrl;
            mevcutUrun.UrunSeo = urun.UrunSeo;
            mevcutUrun.UrunTaglar = urun.UrunTaglar;
            mevcutUrun.UrunFiyat = urun.UrunFiyat;
            mevcutUrun.UrunAciklama = urun.UrunAciklama;
            mevcutUrun.TeknikOzellikler = urun.TeknikOzellikler;
            mevcutUrun.Stok = urun.Stok;
            mevcutUrun.KategoriId = urun.KategoriId;
            mevcutUrun.MarkaId = urun.MarkaId;
            mevcutUrun.GuncelleyenUyeId = urun.GuncelleyenUyeId;
            mevcutUrun.GuncellemeTarihi = DateTime.Now;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var urun = await _context.TblUrun.FindAsync(id);
            if (urun == null) return NotFound();

            _context.TblUrun.Remove(urun);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
