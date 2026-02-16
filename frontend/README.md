# Frontend - Medical Appointments Application

Applicazione React per la gestione di appuntamenti medici, parte del project work universitario.

## Stack Tecnologico

- **Framework**: React 19.2
- **Build Tool**: Vite 7.3
- **Linguaggio**: JavaScript (ES6+)
- **UI Library**: Material-UI (MUI) 7.3
- **Routing**: React Router v7
- **HTTP Client**: Axios
- **Form Management**: React Hook Form
- **State Management**: React Context API
- **Styling**: Material-UI + Custom CSS

## Funzionalità Principali

### Per Pazienti
- ✅ Registrazione e autenticazione
- ✅ Dashboard personalizzata
- ✅ Ricerca e visualizzazione medici per specializzazione
- ✅ Prenotazione appuntamenti (wizard multi-step)
- ✅ Gestione appuntamenti (visualizza, modifica, cancella)
- ✅ Visualizzazione referti medici
- ✅ Profilo utente

### Per Personale Medico (Admin/Medico/Receptionist)
- ✅ Gestione pazienti (CRUD completo)
- ✅ Visualizzazione dettagliata pazienti con storico
- ✅ Dashboard con statistiche
- ✅ Gestione appuntamenti
- ✅ Gestione referti

## Prerequisiti

- Node.js 20.10.0+ (consigliato 20.19.0+)
- npm 10.2.3+
- Backend API in esecuzione (vedi `../backend/README.md`)

## Installazione

1. **Entra nella cartella frontend**
   ```bash
   cd frontend
   ```

2. **Installa le dipendenze**
   ```bash
   npm install
   ```

3. **Configura le variabili d'ambiente**
   
   Crea un file `.env` nella root del progetto frontend:
   ```env
   VITE_API_BASE_URL=http://localhost:5000/api
   ```

4. **Avvia il server di sviluppo**
   ```bash
   npm run dev
   ```

   L'applicazione sarà disponibile su `http://localhost:5173`

## Script Disponibili

- `npm run dev` - Avvia il server di sviluppo
- `npm run build` - Crea la build di produzione
- `npm run preview` - Preview della build di produzione
- `npm run lint` - Esegue ESLint per il controllo del codice

## Struttura del Progetto

```
frontend/
├── public/                 # Asset statici
├── src/
│   ├── assets/            # Immagini, icone
│   ├── components/        # Componenti React
│   │   ├── auth/          # Componenti autenticazione
│   │   ├── common/        # Componenti riutilizzabili
│   │   ├── layout/        # Layout e navigazione
│   │   ├── pazienti/      # Componenti specifici pazienti
│   │   ├── medici/        # Componenti specifici medici
│   │   ├── appuntamenti/  # Componenti appuntamenti
│   │   └── referti/       # Componenti referti
│   ├── context/           # React Context (state globale)
│   │   ├── AuthContext.jsx
│   │   └── NotificationContext.jsx
│   ├── hooks/             # Custom React hooks
│   │   ├── useFetch.js
│   │   ├── useDebounce.js
│   │   └── usePagination.js
│   ├── pages/             # Pagine/views principali
│   │   ├── LoginPage.jsx
│   │   ├── RegisterPage.jsx
│   │   ├── DashboardPage.jsx
│   │   ├── PazientiPage.jsx
│   │   ├── MediciPage.jsx
│   │   ├── AppuntamentiPage.jsx
│   │   ├── NuovoAppuntamentoPage.jsx
│   │   └── RefertiPage.jsx
│   ├── services/          # Servizi API
│   │   ├── api.js                  # Configurazione Axios
│   │   ├── authService.js
│   │   ├── pazienteService.js
│   │   ├── medicoService.js
│   │   ├── appuntamentoService.js
│   │   └── refertoService.js
│   ├── utils/             # Utility functions
│   │   ├── dateUtils.js
│   │   └── validators.js
│   ├── styles/            # CSS globali
│   ├── App.jsx            # Componente principale
│   ├── main.jsx           # Entry point
│   └── index.css          # Stili globali
├── .env                   # Variabili d'ambiente
├── .env.example           # Template variabili d'ambiente
├── index.html             # HTML template
├── package.json           # Dipendenze e scripts
└── vite.config.js         # Configurazione Vite
```

## Architettura

### Context API

L'applicazione utilizza React Context per la gestione dello stato globale:

- **AuthContext**: Gestione autenticazione e utente corrente
- **NotificationContext**: Sistema di notifiche toast

### Servizi API

Tutti i servizi API sono organizzati in moduli separati:

- `api.js`: Configurazione Axios con interceptors per JWT
- `authService.js`: Login, registrazione, logout
- `pazienteService.js`: CRUD pazienti
- `medicoService.js`: Gestione medici e specializzazioni
- `appuntamentoService.js`: Gestione appuntamenti e disponibilità
- `refertoService.js`: Gestione referti medici

### Custom Hooks

- `useFetch`: Hook per gestire chiamate API asincrone con loading/error states
- `useDebounce`: Debouncing per input di ricerca
- `usePagination`: Gestione paginazione lato client

### Routing

Routing implementato con React Router v7:

- Route pubbliche: `/login`, `/register`
- Route protette: tutte le altre (richiedono autenticazione)
- Autorizzazione basata su ruoli per alcune route

### Componenti Riutilizzabili

- `Button`: Bottone personalizzato con stato loading
- `Card`: Card container
- `Modal`: Modale personalizzata
- `Table`: Tabella con paginazione e sorting
- `SearchBar`: Barra di ricerca con debounce
- `Spinner`: Loading indicator
- `Notification`: Sistema toast per notifiche

## Autenticazione

L'applicazione utilizza JWT (JSON Web Tokens) per l'autenticazione:

1. Login: l'utente invia credenziali
2. Backend restituisce JWT token
3. Token salvato in localStorage
4. Token incluso automaticamente in tutte le richieste API (via Axios interceptor)
5. Logout: rimozione token da localStorage

## Gestione Errori

- Interceptor Axios per gestire errori 401 (token scaduto) → redirect a login
- Componente Notification per mostrare messaggi di errore/successo
- Try-catch in tutti i componenti per gestire errori specifici

## Responsive Design

L'applicazione è completamente responsive:

- Mobile: menu hamburger, layout a colonna singola
- Tablet: layout adattivo
- Desktop: sidebar fissa, layout multi-colonna

Breakpoints MUI:
- xs: 0px
- sm: 600px
- md: 900px
- lg: 1200px
- xl: 1536px

## Testing Manuale

### Credenziali di Test

Dopo aver avviato il backend e popolato il database, puoi usare:

```
Username: admin
Password: admin123
```

oppure registra un nuovo utente dalla pagina di registrazione.

### Flusso di Test Consigliato

1. **Registrazione**: Crea un nuovo account paziente
2. **Login**: Accedi con le credenziali create
3. **Dashboard**: Visualizza la dashboard con statistiche
4. **Medici**: Esplora la lista dei medici disponibili
5. **Prenotazione**: Prenota un appuntamento (wizard multi-step)
6. **Appuntamenti**: Visualizza, modifica o cancella appuntamenti
7. **Referti**: Visualizza i referti medici (se disponibili)

## Build di Produzione

Per creare una build ottimizzata per la produzione:

```bash
npm run build
```

I file ottimizzati saranno generati nella cartella `dist/`.

### Deploy

L'applicazione può essere deployata su:

- **Vercel**: `vercel deploy`
- **Netlify**: Drag & drop della cartella `dist`
- **GitHub Pages**: Con configurazione appropriata
- **Server proprio**: Servire i file dalla cartella `dist`

Assicurati di configurare correttamente `VITE_API_BASE_URL` per l'ambiente di produzione.

## Variabili d'Ambiente

### Sviluppo (`.env`)

```env
VITE_API_BASE_URL=http://localhost:5000/api
```

### Produzione (`.env.production`)

```env
VITE_API_BASE_URL=https://your-api-domain.com/api
```

## Troubleshooting

### Errore di connessione API

Verifica che:
1. Il backend sia in esecuzione
2. L'URL in `.env` sia corretto
3. CORS sia configurato correttamente nel backend

### Errore 401 Unauthorized

- Token JWT scaduto → Effettua nuovamente il login
- Token non valido → Cancella localStorage e rieffettua login

### Problemi con npm install

Se riscontri errori durante `npm install`:
```bash
# Pulisci la cache npm
npm cache clean --force

# Rimuovi node_modules
rm -rf node_modules package-lock.json

# Reinstalla
npm install
```

## Snippet di Codice Interessanti

### 1. Axios Interceptor per JWT

```javascript
// src/services/api.js
api.interceptors.request.use((config) => {
  const token = localStorage.getItem('token');
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});
```

### 2. Custom Hook useFetch

```javascript
// src/hooks/useFetch.js
export const useFetch = (asyncFunction) => {
  const [data, setData] = useState(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  const execute = async (...args) => {
    setLoading(true);
    try {
      const result = await asyncFunction(...args);
      setData(result);
      return result;
    } catch (err) {
      setError(err.message);
      throw err;
    } finally {
      setLoading(false);
    }
  };

  return { data, loading, error, execute };
};
```

### 3. Protected Route con React Router

```javascript
// src/components/auth/ProtectedRoute.jsx
const ProtectedRoute = ({ children, allowedRoles = [] }) => {
  const { user, loading } = useAuth();
  
  if (loading) return <Spinner />;
  if (!user) return <Navigate to="/login" />;
  if (allowedRoles.length > 0 && !allowedRoles.includes(user.ruolo)) {
    return <Navigate to="/" />;
  }
  
  return children;
};
```

### 4. Wizard Multi-Step per Prenotazione

```javascript
// src/pages/NuovoAppuntamentoPage.jsx
const steps = ['Specializzazione', 'Medico', 'Data/Ora', 'Conferma'];

const renderStepContent = () => {
  switch (activeStep) {
    case 0: return <SpecializzazioneStep />;
    case 1: return <MedicoStep />;
    case 2: return <DataOraStep />;
    case 3: return <ConfermaStep />;
  }
};
```

## Miglioramenti Futuri

- [ ] Implementare PWA (Progressive Web App)
- [ ] Aggiungere notifiche push
- [ ] Implementare chat con medico
- [ ] Aggiungere calendario visualizzazione
- [ ] Implementare upload documenti/allegati
- [ ] Aggiungere test automatici (Jest + React Testing Library)
- [ ] Implementare dark mode
- [ ] Aggiungere internazionalizzazione (i18n)
- [ ] Implementare cache con Service Worker
- [ ] Aggiungere analytics

## Risorse Utili

- [React Documentation](https://react.dev/)
- [Material-UI Documentation](https://mui.com/)
- [React Router Documentation](https://reactrouter.com/)
- [Vite Documentation](https://vitejs.dev/)
- [Axios Documentation](https://axios-http.com/)
- [React Hook Form](https://react-hook-form.com/)

## Licenza

Progetto universitario - Corso di Informatica per le Aziende Digitali (L-31)

## Autore

Studente: [Nome Cognome]  
Anno Accademico: 2025/2026  
Project Work: Sviluppo applicazione full-stack per settore sanitario

---

**Nota**: Questo progetto è stato sviluppato come parte di un esame universitario e non è destinato all'uso in produzione senza ulteriori test e implementazioni di sicurezza.
