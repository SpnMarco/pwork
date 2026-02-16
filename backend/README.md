# Medical Appointments API - Backend

API RESTful per la gestione di appuntamenti medici, pazienti, medici e referti.

## Stack Tecnologico

- **Framework**: ASP.NET Core 8.0 Web API
- **Linguaggio**: C# 12
- **Database**: SQLite (file-based, zero configuration)
- **ORM**: Entity Framework Core 8.0
- **Autenticazione**: JWT (JSON Web Tokens)
- **Documentazione API**: Swagger/OpenAPI
- **Pattern**: Repository Pattern + Service Layer

## Struttura del Progetto

```
backend/
├── Controllers/        # Endpoint API
├── Models/            # Entità del dominio
├── DTOs/              # Data Transfer Objects
├── Services/          # Logica di business
├── Repositories/      # Accesso ai dati
├── Data/              # DbContext e configurazioni
├── Interfaces/        # Contratti/interfacce
└── Middleware/        # Middleware personalizzati
```

## Modelli del Dominio

### Entità Principali

1. **Paziente**: Informazioni anagrafiche dei pazienti
2. **Medico**: Informazioni sui medici e specializzazioni
3. **Specializzazione**: Specializzazioni mediche disponibili
4. **Appuntamento**: Prenotazioni visite mediche
5. **Referto**: Referti medici associati agli appuntamenti
6. **DisponibilitaMedico**: Orari di disponibilità dei medici
7. **Utente**: Gestione autenticazione e autorizzazione

### Diagramma ER

```
Paziente 1---* Appuntamento *---1 Medico
    |                              |
    |                              |
    1                              1
    |                              |
    *                              *
Referto                    Specializzazione
    |
    |
    1
    |
    1
Appuntamento
```

## Setup e Installazione

### Prerequisiti

- .NET 8.0 SDK
- Visual Studio 2022 / VS Code / Rider (opzionale)

### Installazione

1. **Clona il repository**
   ```bash
   git clone <repository-url>
   cd backend
   ```

2. **Ripristina i pacchetti NuGet**
   ```bash
   dotnet restore
   ```

3. **Avvia l'applicazione**
   ```bash
   dotnet run
   ```

   Il database SQLite verrà creato automaticamente con dati di seed al primo avvio!

4. **Accedi alla documentazione Swagger**
   ```
   http://localhost:5098/swagger
   ```

### 🎉 Database Pre-popolato!

L'applicazione include un sistema di seed automatico che crea:
- ✅ 5 Pazienti
- ✅ 5 Medici (uno per specializzazione)
- ✅ 5 Specializzazioni
- ✅ 12 Utenti (Admin, Receptionist, Pazienti, Medici)
- ✅ 10 Appuntamenti (vari stati)
- ✅ 3 Referti medici
- ✅ ~25 Slot di disponibilità medici

**Credenziali per testing:**
- Admin: `admin` / `Admin123!`
- Receptionist: `receptionist` / `Recep123!`
- Paziente: `mario.rossi` / `Mario123!`
- Medico: `dr.caruso` / `Cardio123!`

📖 Vedi `docs/migrazione-sqlite.md` per la lista completa di credenziali e dati di test.
   
## Configurazione

### appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=medical_appointments.db"
  },
  "JwtSettings": {
    "SecretKey": "YourSuperSecretKeyForJwtTokenGeneration12345!",
    "Issuer": "MedicalAppointmentsAPI",
    "Audience": "MedicalAppointmentsClient",
    "ExpirationMinutes": 1440
  }
}
```

### Variabili d'Ambiente (Produzione)

Per la produzione, usa variabili d'ambiente invece di hardcodare le chiavi:

```bash
export ConnectionStrings__DefaultConnection="..."
export JwtSettings__SecretKey="..."
```

## API Endpoints

### Autenticazione

- `POST /api/auth/login` - Login utente
- `POST /api/auth/register` - Registrazione nuovo paziente

### Pazienti (TODO)

- `GET /api/pazienti` - Lista pazienti
- `GET /api/pazienti/{id}` - Dettaglio paziente
- `POST /api/pazienti` - Crea paziente
- `PUT /api/pazienti/{id}` - Aggiorna paziente
- `DELETE /api/pazienti/{id}` - Elimina paziente

### Medici (TODO)

- `GET /api/medici` - Lista medici
- `GET /api/medici/{id}` - Dettaglio medico
- `GET /api/medici/specializzazione/{id}` - Medici per specializzazione

### Appuntamenti (TODO)

- `GET /api/appuntamenti` - Lista appuntamenti
- `GET /api/appuntamenti/{id}` - Dettaglio appuntamento
- `POST /api/appuntamenti` - Prenota appuntamento
- `PUT /api/appuntamenti/{id}` - Modifica appuntamento
- `DELETE /api/appuntamenti/{id}` - Cancella appuntamento
- `GET /api/appuntamenti/disponibilita` - Slot disponibili

### Health Check

- `GET /api/health` - Verifica stato API

## Autenticazione JWT

### Login

**Request:**
```json
POST /api/auth/login
{
  "username": "mario.rossi",
  "password": "password123"
}
```

**Response:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "username": "mario.rossi",
  "email": "mario.rossi@email.com",
  "ruolo": "Paziente",
  "pazienteId": 1
}
```

### Utilizzo del Token

Includi il token nell'header Authorization:

```
Authorization: Bearer <token>
```

### Ruoli Utente

- **Admin**: Accesso completo al sistema
- **Medico**: Gestione appuntamenti e referti
- **Paziente**: Prenotazione appuntamenti, visualizzazione referti
- **Receptionist**: Gestione appuntamenti per i pazienti

## Database

### Seed Data

Il database viene popolato automaticamente con:

- 5 Specializzazioni mediche predefinite:
  - Cardiologia
  - Dermatologia
  - Ortopedia
  - Pediatria
  - Medicina Generale

### Migrations

Creare una nuova migration:
```bash
dotnet ef migrations add <NomeMigration>
```

Applicare le migrations:
```bash
dotnet ef database update
```

Rollback all'ultima migration:
```bash
dotnet ef database update <NomeMigrationPrecedente>
```

## Testing

### Test Manuali con Swagger

1. Avvia l'applicazione
2. Vai su `https://localhost:5001`
3. Usa l'interfaccia Swagger per testare gli endpoint

### Test con Postman/curl

**Esempio Login:**
```bash
curl -X POST https://localhost:5001/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username":"test","password":"test123"}'
```

**Esempio con Token:**
```bash
curl -X GET https://localhost:5001/api/pazienti \
  -H "Authorization: Bearer <token>"
```

## Sviluppo

### Aggiungere un Nuovo Controller

1. Crea il file in `Controllers/`
2. Eredita da `ControllerBase`
3. Aggiungi attributo `[Route("api/[controller]")]`
4. Implementa gli endpoint con attributi HTTP (`[HttpGet]`, `[HttpPost]`, ecc.)

### Aggiungere una Nuova Entità

1. Crea la classe in `Models/`
2. Aggiungi il `DbSet` in `ApplicationDbContext`
3. Configura le relazioni in `OnModelCreating`
4. Crea una migration
5. Applica la migration

## Sicurezza

### Best Practices Implementate

- ✅ Password hashate con SHA256
- ✅ JWT per autenticazione stateless
- ✅ CORS configurato per frontend specifici
- ✅ HTTPS enforced
- ✅ Validazione input con Data Annotations
- ✅ Protezione endpoint con `[Authorize]`

### TODO Sicurezza

- [ ] Implementare rate limiting
- [ ] Aggiungere logging delle operazioni sensibili
- [ ] Implementare refresh tokens
- [ ] Usare BCrypt invece di SHA256 per password
- [ ] Aggiungere validazione CSRF

## Deployment

### Pubblicazione

```bash
dotnet publish -c Release -o ./publish
```

### Docker (TODO)

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY ./publish .
ENTRYPOINT ["dotnet", "MedicalAppointments.dll"]
```

## Troubleshooting

### Errore di connessione al database

Verifica che SQL Server LocalDB sia installato:
```bash
sqllocaldb info
```

### Errore JWT

Verifica che `JwtSettings:SecretKey` sia configurato in `appsettings.json`

### Errore CORS

Aggiungi l'origine del frontend in `Program.cs`:
```csharp
policy.WithOrigins("http://localhost:3000")
```

## Risorse

- [ASP.NET Core Documentation](https://docs.microsoft.com/aspnet/core)
- [Entity Framework Core](https://docs.microsoft.com/ef/core)
- [JWT.io](https://jwt.io/)
- [Swagger](https://swagger.io/docs/)

## Licenza

Progetto universitario - Corso di Informatica per le Aziende Digitali

## Autore

Studente: [Nome Cognome]
Anno Accademico: 2025/2026
