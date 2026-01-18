# 🛍️ SmartStore Reservation System

Sistem inteligent de rezervare cabine de probă pentru magazine de modă. Clienții pot rezerva cabine pentru a proba produse la o dată și oră specifică.

## 📚 Kurulum Rehberleri

### 🎯 IDE'ye Göre Seç:
- **🎨 [Visual Studio 2022](VISUAL_STUDIO_KURULUM.md)** - Tam özellikli IDE (Önerilen)
- **💻 [Visual Studio Code](VSCODE_KURULUM.md)** - Hafif ve hızlı editör
- **🔧 [JetBrains Rider](KURULUM_REHBERI.md)** - Profesyonel IDE

### ⚡ Hızlı Başlangıç:
- **🚀 [5 Dakikada Çalıştır](HIZLI_BASLANGIC.md)** - Deneyimli kullanıcılar için
- **📖 [Detaylı Adım Adım Rehber](KURULUM_ADIM_ADIM.md)** - Yeni başlayanlar için
- **🔍 [Kontrol Listesi](KONTROL_LISTESI.md)** - Neyin kurulu olduğunu kontrol et

## 📋 Caracteristici

### 🔐 Autentificare & Înregistrare
- Sistem complet de înregistrare utilizatori
- Autentificare securizată cu hash SHA256
- Validare email și parolă (minim 6 caractere)
- Sesiune utilizator cu localStorage

### 🛒 Catalog Produse
- 12 produse premium cu imagini Unsplash
- Filtrare după categorii (Rochii, Costume, Casual)
- Detalii complete: preț, mărime, culoare, stoc
- Design responsive și modern

### 📅 Sistem Rezervări
- Rezervare cabină pentru probă produse
- Selectare dată și oră
- Verificare disponibilitate cabine în timp real
- Cod de acces unic pentru fiecare rezervare
- Durată rezervare: 30 minute

### 👤 Panou Utilizator
- Vizualizare rezervări personale
- Detalii rezervare: produs, dată, oră, cabină, cod acces
- Anulare rezervări active
- Status rezervări: Confirmată, În Așteptare, Anulată

## 🏗️ Arhitectură

### Backend (.NET 8 + SQL Server)
```
SmartStoreReservation/
├── API/                          # ASP.NET Core Web API
│   ├── Controllers/              # API Controllers
│   │   ├── AuthController.cs     # Autentificare & Înregistrare
│   │   ├── ProductsController.cs # Gestionare produse
│   │   └── ReservationsController.cs # Gestionare rezervări
│   ├── Middleware/               # Global Exception Handler
│   └── Program.cs                # Configurare aplicație
│
├── Core/                         # Domain Layer
│   ├── Entities.cs               # Entități domeniu
│   ├── DTOs/                     # Data Transfer Objects
│   │   ├── AuthDTOs.cs          # Login, Register, User
│   │   ├── ProductDTOs.cs       # Product, CreateProduct, UpdateProduct
│   │   └── ReservationDTOs.cs   # Reservation, CreateReservation, AvailableCabin
│   ├── Interfaces/               # Repository Pattern
│   └── Mappings/                 # AutoMapper Profiles
│
├── Data/                         # Data Access Layer
│   ├── AppDbContext.cs          # Entity Framework DbContext
│   ├── Repositories/            # Generic Repository
│   ├── UnitOfWork.cs            # Unit of Work Pattern
│   ├── SeedDataService.cs       # Date inițiale
│   └── Migrations/              # EF Core Migrations
│
└── Services/                     # Business Logic Layer
    ├── AuthService.cs           # Logică autentificare
    ├── ProductService.cs        # Logică produse
    └── ReservationService.cs    # Logică rezervări
```

### Frontend (HTML + CSS + JavaScript)
```
Client/
├── index.html                    # Pagină principală
├── app.js                        # Logică aplicație
└── Dockerfile                    # Container frontend
```

### Baza de Date (SQL Server)
```
SmartStoreReservation Database
├── users                         # Utilizatori
├── shops                         # Magazine
├── products                      # Produse
├── categories                    # Categorii
├── product_categories            # Relație Many-to-Many
├── cabins                        # Cabine probă
└── reservations                  # Rezervări
```

## � Instalare și Rulare

### Cerințe Preliminare
- **.NET 8 SDK** - [Download](https://dotnet.microsoft.com/download/dotnet/8.0)
- **SQL Server** (Express sau LocalDB) - [Download](https://www.microsoft.com/sql-server/sql-server-downloads)
- **Python 3.x** - Pentru server HTTP frontend
- **Git** - Pentru clonare repository

### Pași Instalare

#### 1. Clonează Repository
```bash
git clone https://github.com/Tenswe/SmartStoreReservation-.git
cd SmartStoreReservation-
```

#### 2. Configurare Bază de Date

**Connection String** (în `API/appsettings.json`):
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=SmartStoreReservation;Integrated Security=true;TrustServerCertificate=true;MultipleActiveResultSets=true"
  }
}
```

**Creează baza de date:**
```bash
# Instalează EF Tools (doar prima dată)
dotnet tool install --global dotnet-ef

# Aplică migrările
dotnet ef database update --project Data --startup-project API
```

#### 3. Rulează Aplicația

**Terminal 1 - Backend API:**
```bash
cd API
dotnet run
```
API va rula pe: **http://localhost:5269**

**Terminal 2 - Frontend:**
```bash
cd Client
python -m http.server 3000
```
Frontend va rula pe: **http://localhost:3000**

## 🎯 Utilizare

### 1. Înregistrare Utilizator Nou
- Accesează http://localhost:3000
- Click pe "Conectare" → "Înregistrează-te"
- Completează: Nume, Email, Parolă (min 6 caractere)
- Click "Înregistrează-te"

### 2. Autentificare
**Utilizatori Demo (deja în baza de date):**
- Email: `ana.popescu@email.com` | Parolă: `parola123`
- Email: `maria.ionescu@email.com` | Parolă: `parola123`
- Email: `elena.dumitrescu@email.com` | Parolă: `parola123`

### 3. Rezervare Cabină
- Navighează prin catalog produse
- Click pe "Rezervare Cabină" pentru produsul dorit
- Selectează data și ora
- Alege cabina disponibilă
- Confirmă rezervarea
- Primești cod de acces unic

### 4. Vizualizare Rezervări
- Click pe "Rezervările Mele" în meniu
- Vezi toate rezervările tale
- Anulează rezervări active dacă este necesar

## 🔧 API Endpoints

### Autentificare
```
POST /api/v1/auth/register    # Înregistrare utilizator nou
POST /api/v1/auth/login       # Autentificare utilizator
```

### Produse
```
GET  /api/v1/products          # Lista produse (cu filtru categorie opțional)
GET  /api/v1/products/{id}     # Detalii produs
POST /api/v1/products          # Creare produs (admin)
PUT  /api/v1/products/{id}     # Actualizare produs (admin)
```

### Rezervări
```
POST   /api/v1/reservations                    # Creare rezervare
GET    /api/v1/reservations                    # Lista toate rezervările
GET    /api/v1/reservations/user/{userId}      # Rezervări utilizator
DELETE /api/v1/reservations/{id}               # Anulare rezervare
GET    /api/v1/reservations/available-cabins   # Cabine disponibile
```

### Swagger Documentation
Accesează **http://localhost:5269** pentru documentație API interactivă.

## 📊 Date Inițiale (Seed Data)

La prima rulare, aplicația încarcă automat:
- **3 Magazine**: București, Cluj, Timișoara
- **6 Categorii**: Rochii Elegante, Costume Business, Ținute Casual, etc.
- **12 Produse**: Cu imagini reale Unsplash
- **12 Cabine**: Distribuite în cele 3 magazine
- **3 Utilizatori Demo**: Cu parola `parola123`
- **3 Rezervări Demo**: Pentru testare

## 🛠️ Tehnologii Utilizate

### Backend
- **ASP.NET Core 8** - Framework web
- **Entity Framework Core 8** - ORM
- **SQL Server** - Bază de date
- **AutoMapper** - Object mapping
- **Swagger/OpenAPI** - Documentație API

### Frontend
- **HTML5** - Structură
- **Tailwind CSS** - Styling (via CDN)
- **Vanilla JavaScript** - Logică client
- **Axios** - HTTP requests

### Patterns & Practices
- **Clean Architecture** - Separare responsabilități
- **Repository Pattern** - Abstractizare acces date
- **Unit of Work** - Gestionare tranzacții
- **DTO Pattern** - Transfer date între layere
- **Dependency Injection** - Loose coupling
- **Global Exception Handling** - Gestionare erori centralizată

## � Structură Bază de Date

### Tabele Principale

**users** - Utilizatori
```sql
id (uniqueidentifier, PK)
name (nvarchar)
email (nvarchar)
password_hash (nvarchar)
measurements (nvarchar)
style_preferences (nvarchar)
created_at (datetime2)
```

**products** - Produse
```sql
id (bigint, PK)
shop_id (bigint, FK)
name (nvarchar)
size (nvarchar)
color (nvarchar)
stock (int)
price (decimal(18,2))
image_url (nvarchar)
created_at (datetime2)
```

**reservations** - Rezervări
```sql
id (bigint, PK)
user_id (uniqueidentifier, FK)
product_id (bigint, FK)
cabin_id (bigint, FK)
date (datetime2)
hour (time)
access_code (nvarchar)
duration (int)
status (nvarchar)
created_at (datetime2)
```

## 🔒 Securitate

- **Hash Parole**: SHA256 pentru stocare securizată
- **Validare Input**: Data Annotations pe toate DTO-urile
- **SQL Injection Protection**: Entity Framework parametrizat
- **CORS**: Configurat pentru localhost development
- **HTTPS**: TrustServerCertificate pentru development

## 🐛 Troubleshooting

### Eroare: "Cannot connect to SQL Server"
```bash
# Verifică dacă SQL Server rulează
# Pentru LocalDB:
sqllocaldb start mssqllocaldb

# Pentru SQL Server Express:
# Services → SQL Server (SQLEXPRESS) → Start
```

### Eroare: "Port 5269 already in use"
```bash
# Oprește procesul care folosește portul
netstat -ano | findstr :5269
taskkill /PID <process_id> /F
```

### Eroare: "Migration not found"
```bash
# Recreează migrările
dotnet ef migrations remove --project Data --startup-project API --force
dotnet ef migrations add InitialCreate --project Data --startup-project API
dotnet ef database update --project Data --startup-project API
```

### Frontend nu se conectează la API
- Verifică că API rulează pe http://localhost:5269
- Verifică CORS în `API/Program.cs`
- Verifică `API_URL` în `Client/index.html`

## 📝 Comenzi Utile

```bash
# Build proiect
dotnet build

# Run cu watch (auto-reload)
dotnet watch run --project API

# Șterge baza de date
dotnet ef database drop --project Data --startup-project API --force

# Creare migrare nouă
dotnet ef migrations add <MigrationName> --project Data --startup-project API

# Aplică migrări
dotnet ef database update --project Data --startup-project API

# Verifică migrări
dotnet ef migrations list --project Data --startup-project API
```

## 🎓 Criterii Academice Îndeplinite

✅ **Clean Architecture** - Separare în layere (API, Core, Data, Services)  
✅ **Repository Pattern** - Abstractizare acces date  
✅ **Unit of Work** - Gestionare tranzacții  
✅ **DTO Pattern** - Transfer date sigur  
✅ **Dependency Injection** - Loose coupling  
✅ **AutoMapper** - Object mapping  
✅ **Global Exception Handler** - Gestionare erori centralizată  
✅ **Entity Framework Core** - ORM modern  
✅ **RESTful API** - Endpoints standard  
✅ **Swagger Documentation** - API documentation  

## 📄 Licență

Acest proiect este dezvoltat pentru scopuri educaționale.

## 👨‍💻 Autor

Proiect dezvoltat ca parte a cursului de Dezvoltare Aplicații Web.

## 🔗 Link-uri Utile

- [.NET 8 Documentation](https://learn.microsoft.com/en-us/dotnet/core/whats-new/dotnet-8)
- [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)
- [SQL Server Documentation](https://learn.microsoft.com/en-us/sql/sql-server/)
- [Tailwind CSS](https://tailwindcss.com/)
- [Repository GitHub](https://github.com/Tenswe/SmartStoreReservation-)

---

**Versiune:** 2.0  
**Ultima actualizare:** Ianuarie 2026  
**Status:** ✅ Production Ready
