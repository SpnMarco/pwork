# Medical Appointments - Project Work

Sistema full-stack per la gestione di appuntamenti medici, pazienti, medici e referti.

**Corso**: Informatica per le Aziende Digitali (L-31)  
**Anno Accademico**: 2025/2026  
**Tema**: La digitalizzazione dell'impresa - Settore Sanitario

---

## 📋 Descrizione Progetto

Applicazione web full-stack API-based per un'organizzazione del settore sanitario che permette di:

- 👥 Gestire pazienti e medici
- 📅 Prenotare e gestire appuntamenti medici
- 🏥 Consultare specializzazioni disponibili
- 📄 Creare e visualizzare referti medici
- 🔐 Sistema di autenticazione multi-ruolo (Admin, Medico, Paziente, Receptionist)

---

## 🏗️ Architettura

### Stack Tecnologico

#### Backend
- **Framework**: ASP.NET Core 8.0 Web API
- **Linguaggio**: C# 12
- **Database**: SQLite (file-based, zero configuration)
- **ORM**: Entity Framework Core 8.0
- **Autenticazione**: JWT (JSON Web Tokens)
- **Documentazione**: Swagger/OpenAPI
- **Pattern**: Repository Pattern + Service Layer

#### Frontend (Da Implementare)
- **Framework**: React 18+
- **Linguaggio**: JavaScript (ES6+)
- **Build Tool**: Vite
- **HTTP Client**: Axios
- **Routing**: React Router v6
- **UI Library**: Material-UI / Ant Design
- **State Management**: Context API

---

## 📁 Struttura Progetto

```
pwork/
├── backend/                    # ✅ Backend ASP.NET Core
│   ├── Controllers/           # API Endpoints
│   ├── Models/                # Entità del dominio (7 modelli)
│   ├── DTOs/                  # Data Transfer Objects
│   ├── Services/              # Logica di business
│   ├── Repositories/          # Accesso ai dati
│   ├── Data/                  # DbContext
│   ├── Interfaces/            # Contratti
│   ├── Middleware/            # Middleware custom
│   ├── Program.cs             # Configurazione app
│   ├── appsettings.json       # Configurazioni
│   └── README.md              # Documentazione backend
│
├── frontend/                   # 🔜 Frontend React (da implementare)
│   ├── src/
│   │   ├── components/
│   │   ├── pages/
│   │   ├── services/
│   │   ├── context/
│   │   └── App.jsx
│   └── package.json
│
└── docs/                       # 📚 Documentazione
    ├── istruzioni-progetto.md           # Traccia project work
    ├── piano-sviluppo-backend.md        # Piano dettagliato backend
    ├── piano-sviluppo-frontend.md       # Piano dettagliato frontend
    ├── setup-completato-backend.md      # Riepilogo setup
    └── prossimi-step-backend.md         # Roadmap sviluppo
```

---

## 🚀 Quick Start

### Opzione 1: Docker (Consigliato per Deploy)

```bash
# Avvia tutto con Docker Compose
docker-compose up --build

# Accesso:
# Frontend: http://localhost:3000
# Backend: http://localhost:8080
# Swagger: http://localhost:8080/swagger
```

📚 **Guida completa:** [`DOCKER_README.md`](./DOCKER_README.md)

### Opzione 2: Sviluppo Locale

#### Backend

**Prerequisiti:**
- .NET 8.0 SDK

**Avvio:**
```bash
# Naviga nella cartella backend
cd backend

# Ripristina i pacchetti
dotnet restore

# Avvia l'applicazione
dotnet run

# L'API sarà disponibile su http://localhost:5098
# Swagger UI: http://localhost:5098/swagger
```

#### Test API

Il database viene popolato automaticamente con dati di esempio all'avvio!

**Credenziali per testing:**
- Admin: `admin` / `Admin123!`
- Receptionist: `receptionist` / `Recep123!`
- Paziente: `mario.rossi` / `Mario123!`
- Medico: `dr.caruso` / `Cardio123!`

Vedi `docs/migrazione-sqlite.md` per la lista completa.

```bash
# Health check
curl http://localhost:5098/api/health

# Login
curl -X POST http://localhost:5098/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username":"mario.rossi","password":"Mario123!"}'
```

#### Frontend

**Prerequisiti:**
- Node.js 20+

**Avvio:**
```bash
# Naviga nella cartella frontend
cd frontend

# Installa dipendenze
npm install

# Avvia il dev server
npm run dev

# L'app sarà disponibile su http://localhost:5173
```

---

## 🐳 Docker & Deployment

### Test Locale con Docker

```bash
# Build e avvia tutti i servizi
docker-compose up --build

# In background
docker-compose up -d --build

# Visualizza logs
docker-compose logs -f
```
---

## 📊 Modelli del Dominio

### Entità Implementate

1. **Paziente** - Informazioni anagrafiche pazienti
2. **Medico** - Dati medici e specializzazioni
3. **Specializzazione** - Specializzazioni mediche (5 predefinite)
4. **Appuntamento** - Prenotazioni visite
5. **Referto** - Referti medici
6. **DisponibilitaMedico** - Orari disponibilità
7. **Utente** - Autenticazione e autorizzazione

### Relazioni
```
Paziente 1---* Appuntamento *---1 Medico
    |              |              |
    |              |              |
    *              1              *
    |              |              |
Referto ←---------┘      Specializzazione
```

---

## 🔐 Autenticazione

### Ruoli Utente
- **Admin**: Accesso completo al sistema
- **Medico**: Gestione appuntamenti e referti
- **Paziente**: Prenotazione appuntamenti, visualizzazione referti
- **Receptionist**: Gestione appuntamenti per i pazienti

### JWT Token
- Algoritmo: HS256
- Scadenza: 24 ore
- Include: UserId, Username, Email, Ruolo, PazienteId/MedicoId

---

## 📡 API Endpoints

### ✅ Implementati

#### Autenticazione
- `POST /api/auth/login` - Login utente
- `POST /api/auth/register` - Registrazione paziente

#### Health Check
- `GET /api/health` - Stato API

#### Pazienti
- `GET /api/pazienti` - Lista pazienti
- `GET /api/pazienti/{id}` - Dettaglio
- `POST /api/pazienti` - Crea
- `PUT /api/pazienti/{id}` - Aggiorna
- `DELETE /api/pazienti/{id}` - Elimina

#### Medici
- `GET /api/medici` - Lista medici
- `GET /api/medici/{id}` - Dettaglio
- `GET /api/medici/specializzazione/{id}` - Per specializzazione

#### Appuntamenti
- `GET /api/appuntamenti` - Lista
- `POST /api/appuntamenti` - Prenota
- `GET /api/appuntamenti/disponibilita` - Slot disponibili

#### Referti
- `GET /api/referti` - Lista
- `POST /api/referti` - Crea
- `GET /api/referti/{id}/download` - Download PDF
