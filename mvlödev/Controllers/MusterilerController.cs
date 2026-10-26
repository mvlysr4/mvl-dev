using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mvlodev.Data;
using mvlodev.Models;

namespace mvlodev.Controllers
{
    // Slayttaki gibi metot ismiyle adres erişimi sağlar (Örn: api/Musteriler/GetMusteriler)[cite: 25]
    [Route("api/[controller]/[action]")]
    [ApiController] // Bu sınıfın bir API Controller olduğunu belirtir[cite: 25]
    public class MusterilerController : ControllerBase
    {
        // Dependency Injection için veritabanı bağlamı (DbContext) değişkeni[cite: 25]
        private readonly mvlodevAPIDbContext _dbContext;

        // Constructor: mvlodevAPIDbContext nesnesi dışarıdan enjekte edilir[cite: 25]
        public MusterilerController(mvlodevAPIDbContext dbContext)
        {
            _dbContext = dbContext; // Veritabanı nesnesi yerel değişkene atanır[cite: 25]
        }

        // ==========================================
        // 1. TÜM MÜŞTERİLERİ LİSTELE (GET)
        // ==========================================
        [HttpGet] // HTTP GET isteğini karşılar[cite: 12, 27]
        public async Task<IActionResult> GetMusteriler()
        {
            // Veritabanındaki tüm müşterileri liste halinde asenkron olarak getirir[cite: 27]
            return Ok(await _dbContext.Musteriler.ToListAsync());
        }

        // ==========================================
        // 2. MÜŞTERİ EKLEME (INSERT - POST)
        // ==========================================
        [HttpPost] // HTTP POST isteğini karşılar[cite: 12, 31]
        public async Task<IActionResult> AddMusteri([FromBody] Musteri addMusteriRequest)
        {
            // Gelen istek gövdesindeki verilerle yeni bir Musteri nesnesi oluşturulur[cite: 31]
            var musteri = new Musteri()
            {
                MusteriAdi = addMusteriRequest.MusteriAdi,
                MusteriSoyadi = addMusteriRequest.MusteriSoyadi,
                MusteriTelefon = addMusteriRequest.MusteriTelefon,
                MusteriIletisim = addMusteriRequest.MusteriIletisim,
                MusteriCinsiyet = addMusteriRequest.MusteriCinsiyet,
                MusteriAcikAdres = addMusteriRequest.MusteriAcikAdres
            };

            await _dbContext.Musteriler.AddAsync(musteri); // Nesneyi ekleme kuyruğuna alır[cite: 31]
            await _dbContext.SaveChangesAsync(); // Değişiklikleri veritabanına kaydeder[cite: 31]

            return Ok(musteri); // Eklenen müşteri verisini yanıt olarak döner[cite: 31]
        }

        // ==========================================
        // 3. MÜŞTERİ GÜNCELLEME (UPDATE - PUT)
        // ==========================================
        [HttpPut("{id}")] // URL üzerinden güncelleme yapılacak müşterinin id değerini alır[cite: 32]
        public async Task<IActionResult> UpdateMusteri(int id, [FromBody] Musteri updateMusteriRequest)
        {
            // Belirtilen id numarasına sahip müşteriyi veritabanında arar[cite: 32]
            var musteri = await _dbContext.Musteriler.FindAsync(id);

            // Müşteri bulunduysa alanlarını günceller[cite: 32]
            if (musteri != null)
            {
                musteri.MusteriAdi = updateMusteriRequest.MusteriAdi;
                musteri.MusteriSoyadi = updateMusteriRequest.MusteriSoyadi;
                musteri.MusteriTelefon = updateMusteriRequest.MusteriTelefon;
                musteri.MusteriIletisim = updateMusteriRequest.MusteriIletisim;
                musteri.MusteriCinsiyet = updateMusteriRequest.MusteriCinsiyet;
                musteri.MusteriAcikAdres = updateMusteriRequest.MusteriAcikAdres;

                await _dbContext.SaveChangesAsync(); // Güncellenmiş verileri kaydeder[cite: 32]
                return Ok(musteri); // Güncel müşteri nesnesini döner[cite: 32]
            }

            // Müşteri veritabanında bulunamazsa 404 Not Found döner[cite: 32]
            return NotFound();
        }

        // ==========================================
        // 4. MÜŞTERİ SİLME (DELETE)
        // ==========================================
        [HttpDelete("{id}")] // URL üzerinden silinecek id değerini alır[cite: 33]
        public async Task<IActionResult> DeleteMusteri(int id)
        {
            // Silinecek müşteriyi id ile arar[cite: 33]
            var musteri = await _dbContext.Musteriler.FindAsync(id);

            // Kayıt bulunduysa siler[cite: 33]
            if (musteri != null)
            {
                _dbContext.Musteriler.Remove(musteri); // Silme listesine ekler[cite: 33]
                await _dbContext.SaveChangesAsync();   // Veritabanından tamamen siler[cite: 33]
                return Ok(musteri);                    // Silinen kaydı yanıt döner[cite: 33]
            }

            return NotFound(); // Kayıt bulunamazsa 404 hatası döner[cite: 33]
        }

        // ==========================================
        // 5. MÜŞTERİ ID İLE SORGULA (SORGULAMA 1)
        // ==========================================
        [HttpGet("{id}")] // URL üzerinden id parametresi alır[cite: 29]
        public async Task<IActionResult> GetMusteriById([FromRoute] int id)
        {
            // Veritabanından id ile tek bir müşteri arar[cite: 29]
            var musteri = await _dbContext.Musteriler.FindAsync(id);

            if (musteri == null)
            {
                return NotFound(); // Kayıt yoksa 404 döner[cite: 29]
            }

            return Ok(musteri); // Müşteri bulunduysa bilgilerini döner[cite: 29]
        }

        // ==========================================
        // 6. MÜŞTERİ ADI İLE SORGULA (SORGULAMA 2)
        // ==========================================
        [HttpGet("{ad}")] // URL üzerinden ad parametresi alır
        public async Task<IActionResult> GetMusteriByAd([FromRoute] string ad)
        {
            // HATA VE DÜZELTME NOTU (Harf Büyüklüğü Uyumu):
            // HATA: Aramalarda tam eşleşme aranması durumunda müşteri bulunamıyordu.
            // DÜZELTME: Contains(ad) kullanarak girilen metni adında barındıran tüm müşteriler filtreye alındı.

            var sonuclar = await _dbContext.Musteriler
                .Where(m => m.MusteriAdi.Contains(ad)) // Müşteri adında parametredeki metin geçenleri getirir
                .ToListAsync();

            if (sonuclar == null || !sonuclar.Any())
            {
                return NotFound(); // Aranan ada sahip müşteri bulunamazsa 404 döner
            }

            return Ok(sonuclar); // Eşleşen tüm müşterileri liste halinde döner
        }

        // ==========================================
        // 7. MÜŞTERİ TELEFON NUMARASI İLE SORGULA (SORGULAMA 3)
        // ==========================================
        [HttpGet("{telefon}")] // URL üzerinden telefon parametresi alır
        public async Task<IActionResult> GetMusteriByTelefon([FromRoute] string telefon)
        {
            var sonuclar = await _dbContext.Musteriler
                .Where(m => m.MusteriTelefon.Contains(telefon)) // Telefon numarasında parametredeki rakamlar geçenleri getirir
                .ToListAsync();

            if (sonuclar == null || !sonuclar.Any())
            {
                return NotFound(); // Aranan telefon numarasına ait kayıt yoksa 404 döner
            }

            return Ok(sonuclar); // Eşleşen müşteri verilerini döner
        }
    }
}