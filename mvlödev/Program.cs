using Microsoft.EntityFrameworkCore;
using mvlodev.Data;

var builder = WebApplication.CreateBuilder(args);

// HATA VE DÜZELTME NOTU 1 (NuGet & Swagger Yetki Hatası):
// HATA: 'swashbuckle.aspnetcore.swaggergen... yoluna erişim reddedildi.'
// NEDENİ: Visual Studio'nun kilitli bir paket klasörüne yetkisiz erişmeye çalışması.
// DÜZELTME: Visual Studio yönetici olarak çalıştırıldı ve NuGet önbelleği temizlendi.

// HATA VE DÜZELTME NOTU 2 (DbContextOptions Bulunamadı / appsettings Okunamadı Hatası):
// HATA: 'Unable to resolve service for type Microsoft.EntityFrameworkCore.DbContextOptions'
// NEDENİ: appsettings.json dosyasındaki tırnak/virgül hatası veya AddDbContext tanımının eksik kalması.
// DÜZELTME: appsettings.json formatı düzeltildi ve builder.Services.AddDbContext servisi Program.cs dosyasına eklendi.

// appsettings.json dosyasındaki "DefaultConnection" bağlantı cümlesini okuyup DbContext'i servis olarak kaydediyoruz[cite: 22]
builder.Services.AddDbContext<mvlodevAPIDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Controller ve Swagger servislerini projeye ekliyoruz[cite: 13, 15]
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Proje geliştirme modundayken Swagger arayüz ekranını aktif hale getiriyoruz[cite: 15, 28]
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection(); // HTTP isteklerini otomatik HTTPS'e yönlendirir
app.UseAuthorization();    // Yetkilendirme mekanizmasını çalıştırır
app.MapControllers();      // Controller yönlendirmelerini bağlar

app.Run(); // Web API uygulamasını başlatır