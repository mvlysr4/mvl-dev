using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mvlodev.Models
{
    // Veritabanındaki 'Musteriler' tablosunun karşılığı olan model sınıfımız[cite: 10]
    public class Musteri
    {
        // [Key]: Birincil anahtar (Primary Key) olduğunu belirtir[cite: 18]
        // [DatabaseGenerated]: Id'nin otomatik artan (Identity) olmasını sağlar[cite: 18]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } // Müşteri benzersiz kimlik numarası[cite: 18]

        public string MusteriAdi { get; set; } // Müşterinin adı[cite: 10]
        public string MusteriSoyadi { get; set; } // Müşterinin soyadı[cite: 10]
        public string MusteriTelefon { get; set; } // Müşterinin telefon numarası[cite: 10]
        public string MusteriIletisim { get; set; } // Müşterinin ikincil iletişim bilgisi (ör: E-posta)[cite: 10]
        public string MusteriCinsiyet { get; set; } // Müşterinin cinsiyeti[cite: 10]
        public string MusteriAcikAdres { get; set; } // Müşterinin açık adresi[cite: 10]
    }
}