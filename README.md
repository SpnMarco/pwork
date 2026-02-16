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
- **Database**: SQL Server (LocalDB per sviluppo)
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

### Backend

#### Prerequisiti
- .NET 8.0 SDK
- SQL Server o SQL Server LocalDB

#### Installazione e Avvio
```bash
# Naviga nella cartella backend
cd backend

# Ripristina i pacchetti
dotnet restore

# Avvia l'applicazione
dotnet run

# L'API sarà disponibile su http://localhost:5098
# Swagger UI: http://localhost:5098
```

#### Test API
```bash
# Health check
curl http://localhost:5098/api/health

# Registrazione utente
curl -X POST http://localhost:5098/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "username": "mario.rossi",
    "email": "mario.rossi@email.com",
    "password": "Password123!",
    "nome": "Mario",
    "cognome": "Rossi",
    "codiceFiscale": "RSSMRA80A01H501U",
    "dataNascita": "1980-01-01",
    "telefono": "3331234567"
  }'

# Login
curl -X POST http://localhost:5098/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username":"mario.rossi","password":"Password123!"}'
```

### Frontend (Da Implementare)
```bash
# Naviga nella cartella frontend
cd frontend

# Installa dipendenze
npm install

# Avvia dev server
npm run dev

# L'app sarà disponibile su http://localhost:5173
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

### 🔜 Da Implementare

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

---

## 📚 Documentazione

### File Disponibili

1. **`docs/istruzioni-progetto.md`**  
   Traccia ufficiale del project work universitario

2. **`docs/piano-sviluppo-backend.md`**  
   Piano dettagliato sviluppo backend (12 fasi)

3. **`docs/piano-sviluppo-frontend.md`**  
   Piano dettagliato sviluppo frontend (14 fasi)

4. **`docs/setup-completato-backend.md`**  
   Riepilogo setup backend completato

5. **`docs/prossimi-step-backend.md`**  
   Roadmap dettagliata prossimi sviluppi

6. **`backend/README.md`**  
   Documentazione tecnica backend

7. **`backend/test-api-examples.http`**  
   Esempi test API (REST Client)

---

## ✅ Stato Avanzamento

### Backend
- [x] Setup progetto
- [x] Modelli del dominio (7)
- [x] DbContext e configurazione
- [x] Autenticazione JWT
- [x] AuthController
- [x] HealthController
- [x] Swagger/OpenAPI
- [x] Database creato e testato
- [ ] PazientiController
- [ ] MediciController
- [ ] AppuntamentiController
- [ ] RefertiController
- [ ] Service Layer
- [ ] Repository Pattern
- [ ] DTOs e AutoMapper
- [ ] Validazione (FluentValidation)
- [ ] Testing

### Frontend
- [ ] Setup progetto React
- [ ] Routing
- [ ] Autenticazione
- [ ] Pagine principali
- [ ] Componenti UI
- [ ] Integrazione API
- [ ] Gestione stato
- [ ] Responsive design

### Documentazione
- [x] Piano sviluppo backend
- [x] Piano sviluppo frontend
- [x] README backend
- [ ] Diagrammi UML
- [ ] Diagramma ER
- [ ] Screenshot applicazione
- [ ] Rapporto finale

---

## 🎯 Prossimi Step

### Priorità Alta
1. Implementare `PazientiController` con CRUD completo
2. Implementare `MediciController` con gestione disponibilità
3. Implementare `AppuntamentiController` con logica slot
4. Creare DTOs per tutte le entità
5. Implementare `AppuntamentoService` per logica business

### Priorità Media
6. Implementare Repository Pattern
7. Configurare AutoMapper
8. Aggiungere FluentValidation
9. Implementare gestione errori globale
10. Setup progetto frontend React

### Priorità Bassa
11. Unit e Integration Testing
12. Logging con Serilog
13. Diagrammi UML ed ER
14. Screenshot e documentazione finale

---

## 🛠️ Tecnologie e Pattern

### Backend
- ✅ **ASP.NET Core Web API** - Framework REST
- ✅ **Entity Framework Core** - ORM
- ✅ **JWT Authentication** - Sicurezza
- ✅ **Swagger/OpenAPI** - Documentazione
- 🔜 **Repository Pattern** - Accesso dati
- 🔜 **Service Layer** - Logica business
- 🔜 **DTOs** - Separazione concerns
- 🔜 **AutoMapper** - Mapping oggetti
- 🔜 **FluentValidation** - Validazione

### Frontend (Pianificato)
- React 18+
- React Router v6
- Axios
- Context API
- Material-UI / Ant Design
- React Hook Form

---

## 📖 Risorse Utili

### Backend
- [ASP.NET Core Docs](https://docs.microsoft.com/aspnet/core)
- [Entity Framework Core](https://docs.microsoft.com/ef/core)
- [JWT.io](https://jwt.io/)
- [Swagger](https://swagger.io/docs/)

### Frontend
- [React Documentation](https://react.dev/)
- [React Router](https://reactrouter.com/)
- [Axios](https://axios-http.com/)
- [Material-UI](https://mui.com/)

---

## 👨‍💻 Sviluppo

### Convenzioni Git
```bash
# Feature
git checkout -b feature/nome-feature
git commit -m "feat: descrizione feature"

# Bug fix
git checkout -b fix/nome-bug
git commit -m "fix: descrizione fix"

# Documentazione
git commit -m "docs: aggiorna documentazione"
```

### Convenzioni Codice
- **C#**: PascalCase per classi e metodi, camelCase per variabili
- **JavaScript**: camelCase per variabili e funzioni, PascalCase per componenti
- **Commenti**: XML comments per API pubbliche
- **Naming**: Nomi descrittivi in italiano per dominio business

---

## 📝 Deliverable Finali

### Codice
- [x] Repository Git con codice completo
- [x] Backend funzionante
- [ ] Frontend funzionante
- [ ] Database popolato con dati di test

### Documentazione
- [x] README principale
- [x] Piani di sviluppo
- [ ] Diagrammi UML (classi, sequenza)
- [ ] Diagramma ER del database
- [ ] Documentazione API (Swagger export)
- [ ] Screenshot applicazione
- [ ] Descrizione snippet interessanti

### Rapporto Finale
- [ ] Contesto organizzazione
- [ ] Descrizione servizio offerto
- [ ] Aspetti di design (UML, ER)
- [ ] Documentazione API
- [ ] Processo di sviluppo
- [ ] Test funzionali con screenshot

---

## 📞 Contatti

**Progetto Universitario**  
Corso: Informatica per le Aziende Digitali (L-31)  
Anno Accademico: 2025/2026

---

## 📄 Licenza

Progetto didattico - Università

---

**Ultimo aggiornamento**: 16 Febbraio 2026  
**Stato**: Backend Setup Completato ✅ | Frontend Da Iniziare 🔜
