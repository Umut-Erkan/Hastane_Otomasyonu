# Hastane Otomasyonu 🏥

Hastane Otomasyon Sistemi, hasta, doktor, resepsiyonist ve admin rollerine sahip kullanıcıların kendi rollerine ait yönetim panelleri üzerinden işlemlerini gerçekleştirebildiği kapsamlı bir web uygulamasıdır. 

Modern web teknolojileri kullanılarak geliştirilmiş olup, hem frontend hem de backend mimarisi ile güçlü ve ölçeklenebilir bir yapı sunar.

## 🚀 Teknolojiler (Tech Stack)

### Backend (Geliştirme Ortamı: .NET 9)
* **ASP.NET Core Web API:** RESTful API hizmeti.
* **Entity Framework Core (v10.x/v9.x):** Code-First veritabanı modellemesi ve SQL Server entegrasyonu.
* **Redis:** Dağıtık önbellekleme (Caching) ve hızlı sorgular için (StackExchange.Redis ve NRedisStack).
* **JWT (JSON Web Token):** Güvenli role-based (Rol tabanlı) kimlik doğrulama.

### Frontend (Geliştirme Ortamı: React 19)
* **React & Vite:** Hızlı derleme ve modern fonksiyonel tabanlı (Hook destekli) kullanıcı arayüzü.
* **React Router Dom:** Sayfalar arası SPA (Single Page Application) yönlendirmesi.
* **Vanilla CSS:** Özelleştirilmiş modern ve dinamik arayüz tasarımları.

### Altyapı ve Veritabanı
* **Docker & Docker Compose:** Üç ayrı servis (API, UI ve MSSQL) tek bir `docker-compose` dosyası ile konteyner mimarisine alınmıştır.
* **SQL Server:** Veri depolama (En yeni mssql imajı ile çalışmaktadır).

## 🛠️ Rol ve Modüller

1. **Admin Paneli:**
   * Hastane personellerinin (Doktor ve Resepsiyonist) kayıtları ve sistem yetkilerinin yönetimi.
2. **Hasta Paneli:**
   * Hastaya özel güvenli kontrol paneli (Dashboard) ve oturum yönetimi.
   * Yazılmış reçeteleri, ilgili doktoru ve tedavi detaylarını ilaç isimleri üzerinden kolay okunabilir şekilde görüntüleme ekranı (`ReceteGoster`).
3. **Doktor Paneli:**
   * Redis destekli kimlik doğrulama üzerinden performanslı giriş.
   * Belirli bir doktora ait randevu detaylarının takibi.
4. **Resepsiyonist Paneli:**
   * Seçilen randevu doktoruna veya polikliniğe göre randevu defteri görüntüleme.
   * Kullanışlı dashboard arayüzü.

## 📁 Proje Dizin Yapısı

```text
📂 Hastane_Otomasyonu
 ├── 📂 Hastane_API          # .NET Core Web API projesi (Backend)
 │    ├── 📂 Controllers     # Endpoint Tanımlamaları ve Routing (Hasta, Admin, vs.)
 │    ├── 📂 Models & DTO    # Veritabanı tabloları ve frontend'e iletilecek Data Transfer nesneleri
 │    ├── 📂 Redis           # Redis servis kayıt ve yapılandırmaları
 │    └── ...
 ├── 📂 Hastane_UI           # React (Vite tabanlı) Frontend
 │    ├── 📂 src             
 │    │    ├── 📂 Admin      # Admin view ve component'leri (Örn: Create_Doktor.jsx)
 │    │    └── ... 
 └── 📄 docker-compose.yml   # Tüm projeyi Docker ile ayağa kaldırmak için config (.env okuyucu)
```

## ⚙️ Kurulum ve Çalıştırma

Projeyi lokal bilgisayarınızda çalıştırmak için aşağıdaki yolları izleyebilirsiniz. Gerekli portların boş olduğundan (API için `5000` / ve `5070` gibi tanımlı port, UI için `80` ve `5173`) emin olun.

### 1- Docker Compose ile (Konteynerde ayağa kaldırma - Önerilen)

Sisteminizde Docker yüklü ise tek komutla tüm ekosistemi (SQL Server, Backend ve Frontend) çalıştırabilirsiniz:

```bash
# Projenin ana klasöründe terminali açıp aşağıdaki komutu çalıştırın:
docker pull busgn162/hastane_api:v2
docker pull busgn162/hastane_ui:v2
docker-compose up -d --build
```
> Ortam değişkenleri için ana dizinde bir `.env` dosyanız olduğundan (`SQL_PASSWORD` gibi değerlerin tanımlı olduğundan) emin olun.

> **Not:** Gizli değerleri (`RedisConnection:Password`, `JwtSettings:jwtKey`, `AdminSecurity:SecretKey` vb.) `appsettings.json` içine koyup repoya *commit etmeyin*. Lokal geliştirme için `.NET User Secrets` veya ortam değişkenleri kullanın.

Bu işlemden sonra:
* **Frontend (UI):** `http://localhost:80`
* **Backend (API):** `http://localhost:5000` (Docker ayarına göre yönlendirilmişse)
* **MSSQL DB:** `1433` portlarında çalışmaya başlar.

### 2- Lokal (Manuel) Geliştirici Ortamı

Eğer backend ve frontend taraflarında kodu çalışırken modify(düzenleme) etmek istiyorsanız ayrı komut satırları kullanabilirsiniz.

**Backend (.NET 9):**
```bash
cd Hastane_API
dotnet build
dotnet run
```

**Frontend (React/Vite):**
```bash
cd Hastane_UI
npm install
npm run dev
```

> **Güvenlik Notu:** `appsettings.json` üzerinde bulunan Redis ConnectionString, SecretKey veya SQL Server bağlantı şifresi gibi bilgilerinizi açık (public) repolara commitlerken saklamaya dikkat ediniz.
