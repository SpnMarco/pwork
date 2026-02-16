# Guida Rapida - Frontend Medical Appointments

## Avvio Rapido

### 1. Installazione

```bash
cd frontend
npm install
```

### 2. Configurazione

Crea il file `.env`:

```env
VITE_API_BASE_URL=http://localhost:5000/api
```

### 3. Avvio

```bash
npm run dev
```

Apri il browser su: `http://localhost:5173`

## Credenziali di Test

Dopo aver avviato il backend, puoi:

1. **Registrarti** dalla pagina di registrazione
2. Oppure usare credenziali esistenti (se il backend ha seed data)

## Flusso di Test Completo

### 1. Registrazione Nuovo Paziente

1. Vai su `/register`
2. Compila il form con i tuoi dati:
   - Nome, Cognome
   - Codice Fiscale (formato: RSSMRA80A01H501U)
   - Data di nascita
   - Email
   - Telefono
   - Username e Password

3. Clicca "Registrati"

### 2. Login

1. Vai su `/login`
2. Inserisci username e password
3. Clicca "Accedi"

### 3. Dashboard

Dopo il login vedrai:
- Statistiche rapide
- Prossimi appuntamenti
- Azioni rapide

### 4. Esplora Medici

1. Vai su "Medici" nella sidebar
2. Filtra per specializzazione
3. Cerca per nome
4. Visualizza dettagli medico
5. Clicca "Prenota" per prenotare

### 5. Prenota Appuntamento

1. Vai su "Appuntamenti" → "Nuovo Appuntamento"
2. **Step 1**: Scegli specializzazione
3. **Step 2**: Scegli medico
4. **Step 3**: Scegli data e ora
5. **Step 4**: Conferma prenotazione

### 6. Gestisci Appuntamenti

1. Vai su "Appuntamenti"
2. Visualizza lista appuntamenti
3. Clicca su un appuntamento per dettagli
4. Modifica o cancella appuntamento

### 7. Visualizza Referti

1. Vai su "Referti"
2. Visualizza lista referti medici
3. Clicca per dettagli
4. Download PDF (se disponibile)

## Funzionalità per Ruolo

### Paziente
- Dashboard personale
- Visualizza medici
- Prenota appuntamenti
- Gestisci i propri appuntamenti
- Visualizza i propri referti

### Admin/Receptionist/Medico
- Tutto quanto sopra +
- Gestione pazienti (CRUD)
- Visualizza dettagli pazienti
- Gestisce tutti gli appuntamenti
- Gestisce tutti i referti

## Componenti Principali

### Layout
- `Navbar`: Barra di navigazione superiore
- `Sidebar`: Menu laterale (solo per utenti autenticati)
- `Layout`: Wrapper principale

### Pages
- `LoginPage`: Pagina di login
- `RegisterPage`: Pagina di registrazione
- `DashboardPage`: Dashboard principale
- `MediciPage`: Lista medici
- `AppuntamentiPage`: Lista appuntamenti
- `NuovoAppuntamentoPage`: Wizard prenotazione
- `PazientiPage`: Gestione pazienti (admin)
- `RefertiPage`: Lista referti

### Componenti Comuni
- `Button`: Bottone con loading state
- `Table`: Tabella con paginazione
- `Modal`: Modale personalizzata
- `SearchBar`: Barra di ricerca
- `Spinner`: Loading indicator
- `Notification`: Toast notifications

## API Integration

Tutti i servizi API sono in `src/services/`:

```javascript
// Esempio: Prenota appuntamento
import appuntamentoService from './services/appuntamentoService';

const prenotaAppuntamento = async (data) => {
  try {
    const result = await appuntamentoService.create({
      medicoId: 1,
      data: '2026-03-15',
      ora: '10:00',
      note: 'Prima visita'
    });
    console.log('Appuntamento creato:', result);
  } catch (error) {
    console.error('Errore:', error);
  }
};
```

## Debug

### Controllare se il backend è in esecuzione

```bash
curl http://localhost:5000/api/health
```

### Verificare token JWT

Apri DevTools → Application → Local Storage → Controlla `token` e `user`

### Vedere chiamate API

Apri DevTools → Network → Filtra per XHR

### Errori comuni

1. **401 Unauthorized**: Token scaduto, rieffettua login
2. **404 Not Found**: Endpoint API non trovato, verifica URL
3. **500 Server Error**: Problema backend, controlla logs backend
4. **CORS Error**: Configura CORS nel backend

## Build Produzione

```bash
npm run build
```

Output in `dist/` pronto per il deploy.

## Supporto

Per problemi o domande:
1. Controlla README principale
2. Controlla console del browser
3. Controlla logs backend
4. Verifica configurazione `.env`

---

**Buon lavoro! 🚀**
