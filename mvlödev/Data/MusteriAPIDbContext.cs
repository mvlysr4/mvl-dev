using mvlodev.Models;
using Microsoft.EntityFrameworkCore;

namespace mvlodev.Data
{
    // Veritabanı ile uygulama arasında köprü kuran DbContext sınıfımız[cite: 2, 20]
    public class mvlodevAPIDbContext : DbContext
    {
        // Constructor: Dışarıdan veritabanı bağlantı ayarlarını (options) alır[cite: 20]
        public mvlodevAPIDbContext(DbContextOptions options) : base(options)
        {
        }

        // Musteriler tablosunu temsil eden DbSet nesnesi[cite: 20]
        public DbSet<Musteri> Musteriler { get; set; }
    }
}