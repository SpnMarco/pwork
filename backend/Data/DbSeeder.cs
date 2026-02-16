using System.Security.Cryptography;
using System.Text;
using MedicalAppointments.Models;

namespace MedicalAppointments.Data;

/// <summary>
/// Classe per il popolamento iniziale del database con dati di esempio
/// </summary>
public static class DbSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        // Verifica se ci sono già dati
        if (context.Pazienti.Any())
        {
            return; // Database già popolato
        }

        Console.WriteLine("Inizio seeding del database...");

        // 1. PAZIENTI (5 pazienti di esempio)
        var pazienti = new List<Paziente>
        {
            new Paziente
            {
                Id = 1,
                Nome = "Mario",
                Cognome = "Rossi",
                CodiceFiscale = "RSSMRA80A01H501U",
                DataNascita = new DateTime(1980, 1, 1),
                Email = "mario.rossi@email.com",
                Telefono = "3331234567",
                Indirizzo = "Via Roma 10",
                Citta = "Milano",
                CAP = "20100",
                DataRegistrazione = DateTime.UtcNow.AddDays(-60)
            },
            new Paziente
            {
                Id = 2,
                Nome = "Giulia",
                Cognome = "Bianchi",
                CodiceFiscale = "BNCGLI85M50F205X",
                DataNascita = new DateTime(1985, 8, 10),
                Email = "giulia.bianchi@email.com",
                Telefono = "3339876543",
                Indirizzo = "Corso Italia 25",
                Citta = "Roma",
                CAP = "00100",
                DataRegistrazione = DateTime.UtcNow.AddDays(-45)
            },
            new Paziente
            {
                Id = 3,
                Nome = "Luca",
                Cognome = "Verdi",
                CodiceFiscale = "VRDLCU90L15G273Y",
                DataNascita = new DateTime(1990, 7, 15),
                Email = "luca.verdi@email.com",
                Telefono = "3345678901",
                Indirizzo = "Piazza Garibaldi 3",
                Citta = "Napoli",
                CAP = "80100",
                DataRegistrazione = DateTime.UtcNow.AddDays(-30)
            },
            new Paziente
            {
                Id = 4,
                Nome = "Francesca",
                Cognome = "Ferrari",
                CodiceFiscale = "FRRFNC92D45L219S",
                DataNascita = new DateTime(1992, 4, 5),
                Email = "francesca.ferrari@email.com",
                Telefono = "3387654321",
                Indirizzo = "Via Dante 18",
                Citta = "Torino",
                CAP = "10100",
                DataRegistrazione = DateTime.UtcNow.AddDays(-20)
            },
            new Paziente
            {
                Id = 5,
                Nome = "Alessandro",
                Cognome = "Russo",
                CodiceFiscale = "RSSLSN88T20D612Z",
                DataNascita = new DateTime(1988, 12, 20),
                Email = "alessandro.russo@email.com",
                Telefono = "3356789012",
                Indirizzo = "Viale Europa 50",
                Citta = "Firenze",
                CAP = "50100",
                DataRegistrazione = DateTime.UtcNow.AddDays(-15)
            }
        };

        await context.Pazienti.AddRangeAsync(pazienti);
        await context.SaveChangesAsync();
        Console.WriteLine($"✓ Creati {pazienti.Count} pazienti");

        // 2. MEDICI (5 medici, uno per specializzazione)
        var medici = new List<Medico>
        {
            new Medico
            {
                Id = 1,
                Nome = "Andrea",
                Cognome = "Caruso",
                Email = "andrea.caruso@clinic.com",
                Telefono = "0212345678",
                NumeroAlbo = "MI123456",
                Biografia = "Cardiologo con 15 anni di esperienza in cardiologia interventistica e diagnostica avanzata.",
                SpecializzazioneId = 1, // Cardiologia
                Attivo = true,
                DataRegistrazione = DateTime.UtcNow.AddDays(-90)
            },
            new Medico
            {
                Id = 2,
                Nome = "Laura",
                Cognome = "Martini",
                Email = "laura.martini@clinic.com",
                Telefono = "0212345679",
                NumeroAlbo = "MI234567",
                Biografia = "Dermatologa specializzata in dermatologia estetica e oncologica.",
                SpecializzazioneId = 2, // Dermatologia
                Attivo = true,
                DataRegistrazione = DateTime.UtcNow.AddDays(-80)
            },
            new Medico
            {
                Id = 3,
                Nome = "Roberto",
                Cognome = "Colombo",
                Email = "roberto.colombo@clinic.com",
                Telefono = "0212345680",
                NumeroAlbo = "MI345678",
                Biografia = "Ortopedico esperto in chirurgia artroscopica e traumatologia sportiva.",
                SpecializzazioneId = 3, // Ortopedia
                Attivo = true,
                DataRegistrazione = DateTime.UtcNow.AddDays(-70)
            },
            new Medico
            {
                Id = 4,
                Nome = "Maria",
                Cognome = "Romano",
                Email = "maria.romano@clinic.com",
                Telefono = "0212345681",
                NumeroAlbo = "MI456789",
                Biografia = "Pediatra con specializzazione in neonatologia e allergologia pediatrica.",
                SpecializzazioneId = 4, // Pediatria
                Attivo = true,
                DataRegistrazione = DateTime.UtcNow.AddDays(-60)
            },
            new Medico
            {
                Id = 5,
                Nome = "Giuseppe",
                Cognome = "Esposito",
                Email = "giuseppe.esposito@clinic.com",
                Telefono = "0212345682",
                NumeroAlbo = "MI567890",
                Biografia = "Medico di medicina generale con esperienza in cure primarie e medicina preventiva.",
                SpecializzazioneId = 5, // Medicina Generale
                Attivo = true,
                DataRegistrazione = DateTime.UtcNow.AddDays(-50)
            }
        };

        await context.Medici.AddRangeAsync(medici);
        await context.SaveChangesAsync();
        Console.WriteLine($"✓ Creati {medici.Count} medici");

        // 3. DISPONIBILITÀ MEDICI
        var disponibilita = new List<DisponibilitaMedico>();

        // Dr. Caruso (Cardiologo) - Lunedì, Mercoledì, Venerdì
        disponibilita.AddRange(new[]
        {
            new DisponibilitaMedico { MedicoId = 1, GiornoSettimana = DayOfWeek.Monday, OraInizio = new TimeSpan(9, 0, 0), OraFine = new TimeSpan(13, 0, 0), Attivo = true },
            new DisponibilitaMedico { MedicoId = 1, GiornoSettimana = DayOfWeek.Monday, OraInizio = new TimeSpan(14, 0, 0), OraFine = new TimeSpan(18, 0, 0), Attivo = true },
            new DisponibilitaMedico { MedicoId = 1, GiornoSettimana = DayOfWeek.Wednesday, OraInizio = new TimeSpan(9, 0, 0), OraFine = new TimeSpan(13, 0, 0), Attivo = true },
            new DisponibilitaMedico { MedicoId = 1, GiornoSettimana = DayOfWeek.Friday, OraInizio = new TimeSpan(9, 0, 0), OraFine = new TimeSpan(13, 0, 0), Attivo = true }
        });

        // Dr.ssa Martini (Dermatologa) - Martedì, Giovedì
        disponibilita.AddRange(new[]
        {
            new DisponibilitaMedico { MedicoId = 2, GiornoSettimana = DayOfWeek.Tuesday, OraInizio = new TimeSpan(10, 0, 0), OraFine = new TimeSpan(14, 0, 0), Attivo = true },
            new DisponibilitaMedico { MedicoId = 2, GiornoSettimana = DayOfWeek.Tuesday, OraInizio = new TimeSpan(15, 0, 0), OraFine = new TimeSpan(19, 0, 0), Attivo = true },
            new DisponibilitaMedico { MedicoId = 2, GiornoSettimana = DayOfWeek.Thursday, OraInizio = new TimeSpan(10, 0, 0), OraFine = new TimeSpan(14, 0, 0), Attivo = true }
        });

        // Dr. Colombo (Ortopedico) - Lunedì, Mercoledì, Venerdì
        disponibilita.AddRange(new[]
        {
            new DisponibilitaMedico { MedicoId = 3, GiornoSettimana = DayOfWeek.Monday, OraInizio = new TimeSpan(14, 0, 0), OraFine = new TimeSpan(18, 0, 0), Attivo = true },
            new DisponibilitaMedico { MedicoId = 3, GiornoSettimana = DayOfWeek.Wednesday, OraInizio = new TimeSpan(14, 0, 0), OraFine = new TimeSpan(18, 0, 0), Attivo = true },
            new DisponibilitaMedico { MedicoId = 3, GiornoSettimana = DayOfWeek.Friday, OraInizio = new TimeSpan(14, 0, 0), OraFine = new TimeSpan(18, 0, 0), Attivo = true }
        });

        // Dr.ssa Romano (Pediatra) - Tutti i giorni feriali mattina
        disponibilita.AddRange(new[]
        {
            new DisponibilitaMedico { MedicoId = 4, GiornoSettimana = DayOfWeek.Monday, OraInizio = new TimeSpan(9, 0, 0), OraFine = new TimeSpan(13, 0, 0), Attivo = true },
            new DisponibilitaMedico { MedicoId = 4, GiornoSettimana = DayOfWeek.Tuesday, OraInizio = new TimeSpan(9, 0, 0), OraFine = new TimeSpan(13, 0, 0), Attivo = true },
            new DisponibilitaMedico { MedicoId = 4, GiornoSettimana = DayOfWeek.Wednesday, OraInizio = new TimeSpan(9, 0, 0), OraFine = new TimeSpan(13, 0, 0), Attivo = true },
            new DisponibilitaMedico { MedicoId = 4, GiornoSettimana = DayOfWeek.Thursday, OraInizio = new TimeSpan(9, 0, 0), OraFine = new TimeSpan(13, 0, 0), Attivo = true },
            new DisponibilitaMedico { MedicoId = 4, GiornoSettimana = DayOfWeek.Friday, OraInizio = new TimeSpan(9, 0, 0), OraFine = new TimeSpan(13, 0, 0), Attivo = true }
        });

        // Dr. Esposito (Medicina Generale) - Tutti i giorni feriali
        disponibilita.AddRange(new[]
        {
            new DisponibilitaMedico { MedicoId = 5, GiornoSettimana = DayOfWeek.Monday, OraInizio = new TimeSpan(9, 0, 0), OraFine = new TimeSpan(12, 0, 0), Attivo = true },
            new DisponibilitaMedico { MedicoId = 5, GiornoSettimana = DayOfWeek.Monday, OraInizio = new TimeSpan(15, 0, 0), OraFine = new TimeSpan(18, 0, 0), Attivo = true },
            new DisponibilitaMedico { MedicoId = 5, GiornoSettimana = DayOfWeek.Tuesday, OraInizio = new TimeSpan(9, 0, 0), OraFine = new TimeSpan(12, 0, 0), Attivo = true },
            new DisponibilitaMedico { MedicoId = 5, GiornoSettimana = DayOfWeek.Wednesday, OraInizio = new TimeSpan(9, 0, 0), OraFine = new TimeSpan(12, 0, 0), Attivo = true },
            new DisponibilitaMedico { MedicoId = 5, GiornoSettimana = DayOfWeek.Thursday, OraInizio = new TimeSpan(9, 0, 0), OraFine = new TimeSpan(12, 0, 0), Attivo = true },
            new DisponibilitaMedico { MedicoId = 5, GiornoSettimana = DayOfWeek.Friday, OraInizio = new TimeSpan(9, 0, 0), OraFine = new TimeSpan(12, 0, 0), Attivo = true }
        });

        await context.DisponibilitaMedici.AddRangeAsync(disponibilita);
        await context.SaveChangesAsync();
        Console.WriteLine($"✓ Create {disponibilita.Count} disponibilità medici");

        // 4. UTENTI (5 pazienti + 5 medici + 1 admin + 1 receptionist)
        var utenti = new List<Utente>
        {
            // Admin
            new Utente
            {
                Id = 1,
                Username = "admin",
                Email = "admin@clinic.com",
                PasswordHash = HashPassword("Admin123!"),
                Ruolo = RuoloUtente.Admin,
                Attivo = true,
                DataCreazione = DateTime.UtcNow.AddDays(-100)
            },
            // Receptionist
            new Utente
            {
                Id = 2,
                Username = "receptionist",
                Email = "receptionist@clinic.com",
                PasswordHash = HashPassword("Recep123!"),
                Ruolo = RuoloUtente.Receptionist,
                Attivo = true,
                DataCreazione = DateTime.UtcNow.AddDays(-90)
            },
            // Pazienti
            new Utente
            {
                Id = 3,
                Username = "mario.rossi",
                Email = "mario.rossi@email.com",
                PasswordHash = HashPassword("Mario123!"),
                Ruolo = RuoloUtente.Paziente,
                PazienteId = 1,
                Attivo = true,
                DataCreazione = DateTime.UtcNow.AddDays(-60)
            },
            new Utente
            {
                Id = 4,
                Username = "giulia.bianchi",
                Email = "giulia.bianchi@email.com",
                PasswordHash = HashPassword("Giulia123!"),
                Ruolo = RuoloUtente.Paziente,
                PazienteId = 2,
                Attivo = true,
                DataCreazione = DateTime.UtcNow.AddDays(-45)
            },
            new Utente
            {
                Id = 5,
                Username = "luca.verdi",
                Email = "luca.verdi@email.com",
                PasswordHash = HashPassword("Luca123!"),
                Ruolo = RuoloUtente.Paziente,
                PazienteId = 3,
                Attivo = true,
                DataCreazione = DateTime.UtcNow.AddDays(-30)
            },
            new Utente
            {
                Id = 6,
                Username = "francesca.ferrari",
                Email = "francesca.ferrari@email.com",
                PasswordHash = HashPassword("Francesca123!"),
                Ruolo = RuoloUtente.Paziente,
                PazienteId = 4,
                Attivo = true,
                DataCreazione = DateTime.UtcNow.AddDays(-20)
            },
            new Utente
            {
                Id = 7,
                Username = "alessandro.russo",
                Email = "alessandro.russo@email.com",
                PasswordHash = HashPassword("Alessandro123!"),
                Ruolo = RuoloUtente.Paziente,
                PazienteId = 5,
                Attivo = true,
                DataCreazione = DateTime.UtcNow.AddDays(-15)
            },
            // Medici
            new Utente
            {
                Id = 8,
                Username = "dr.caruso",
                Email = "andrea.caruso@clinic.com",
                PasswordHash = HashPassword("Cardio123!"),
                Ruolo = RuoloUtente.Medico,
                MedicoId = 1,
                Attivo = true,
                DataCreazione = DateTime.UtcNow.AddDays(-90)
            },
            new Utente
            {
                Id = 9,
                Username = "dr.martini",
                Email = "laura.martini@clinic.com",
                PasswordHash = HashPassword("Derma123!"),
                Ruolo = RuoloUtente.Medico,
                MedicoId = 2,
                Attivo = true,
                DataCreazione = DateTime.UtcNow.AddDays(-80)
            },
            new Utente
            {
                Id = 10,
                Username = "dr.colombo",
                Email = "roberto.colombo@clinic.com",
                PasswordHash = HashPassword("Orto123!"),
                Ruolo = RuoloUtente.Medico,
                MedicoId = 3,
                Attivo = true,
                DataCreazione = DateTime.UtcNow.AddDays(-70)
            },
            new Utente
            {
                Id = 11,
                Username = "dr.romano",
                Email = "maria.romano@clinic.com",
                PasswordHash = HashPassword("Pedia123!"),
                Ruolo = RuoloUtente.Medico,
                MedicoId = 4,
                Attivo = true,
                DataCreazione = DateTime.UtcNow.AddDays(-60)
            },
            new Utente
            {
                Id = 12,
                Username = "dr.esposito",
                Email = "giuseppe.esposito@clinic.com",
                PasswordHash = HashPassword("Medgen123!"),
                Ruolo = RuoloUtente.Medico,
                MedicoId = 5,
                Attivo = true,
                DataCreazione = DateTime.UtcNow.AddDays(-50)
            }
        };

        await context.Utenti.AddRangeAsync(utenti);
        await context.SaveChangesAsync();
        Console.WriteLine($"✓ Creati {utenti.Count} utenti");

        // 5. APPUNTAMENTI (mix di stati: passati, futuri, confermati, completati)
        var now = DateTime.Now;
        var appuntamenti = new List<Appuntamento>
        {
            // Appuntamenti passati completati
            new Appuntamento
            {
                PazienteId = 1,
                MedicoId = 1,
                DataOra = now.AddDays(-10).Date.AddHours(10),
                DurataMinuti = 30,
                Stato = StatoAppuntamento.Completato,
                MotivoVisita = "Controllo pressione",
                Note = "Paziente in buone condizioni",
                DataCreazione = now.AddDays(-12)
            },
            new Appuntamento
            {
                PazienteId = 2,
                MedicoId = 2,
                DataOra = now.AddDays(-8).Date.AddHours(11),
                DurataMinuti = 30,
                Stato = StatoAppuntamento.Completato,
                MotivoVisita = "Controllo nei",
                Note = "Tutto regolare, controllo consigliato tra 6 mesi",
                DataCreazione = now.AddDays(-10)
            },
            new Appuntamento
            {
                PazienteId = 3,
                MedicoId = 3,
                DataOra = now.AddDays(-5).Date.AddHours(15),
                DurataMinuti = 30,
                Stato = StatoAppuntamento.Completato,
                MotivoVisita = "Dolore ginocchio",
                Note = "Prescritta risonanza magnetica",
                DataCreazione = now.AddDays(-7)
            },
            
            // Appuntamenti passati cancellati
            new Appuntamento
            {
                PazienteId = 4,
                MedicoId = 1,
                DataOra = now.AddDays(-3).Date.AddHours(14),
                DurataMinuti = 30,
                Stato = StatoAppuntamento.Cancellato,
                MotivoVisita = "Controllo cardiologico",
                Note = "Paziente ha cancellato per impegni",
                DataCreazione = now.AddDays(-5)
            },

            // Appuntamenti futuri programmati
            new Appuntamento
            {
                PazienteId = 1,
                MedicoId = 5,
                DataOra = now.AddDays(2).Date.AddHours(9).AddMinutes(30),
                DurataMinuti = 30,
                Stato = StatoAppuntamento.Programmato,
                MotivoVisita = "Visita di controllo generale",
                DataCreazione = now.AddDays(-1)
            },
            new Appuntamento
            {
                PazienteId = 2,
                MedicoId = 1,
                DataOra = now.AddDays(3).Date.AddHours(10),
                DurataMinuti = 30,
                Stato = StatoAppuntamento.Confermato,
                MotivoVisita = "Follow-up cardiologico",
                Note = "Portare esami del sangue",
                DataCreazione = now.AddDays(-2)
            },
            new Appuntamento
            {
                PazienteId = 3,
                MedicoId = 2,
                DataOra = now.AddDays(4).Date.AddHours(11),
                DurataMinuti = 30,
                Stato = StatoAppuntamento.Programmato,
                MotivoVisita = "Visita dermatologica",
                DataCreazione = now.AddDays(-1)
            },
            new Appuntamento
            {
                PazienteId = 4,
                MedicoId = 4,
                DataOra = now.AddDays(5).Date.AddHours(10).AddMinutes(30),
                DurataMinuti = 30,
                Stato = StatoAppuntamento.Confermato,
                MotivoVisita = "Controllo pediatrico",
                Note = "Prima visita",
                DataCreazione = now.AddDays(-3)
            },
            new Appuntamento
            {
                PazienteId = 5,
                MedicoId = 3,
                DataOra = now.AddDays(7).Date.AddHours(15),
                DurataMinuti = 30,
                Stato = StatoAppuntamento.Programmato,
                MotivoVisita = "Dolore spalla",
                DataCreazione = now
            },
            new Appuntamento
            {
                PazienteId = 1,
                MedicoId = 1,
                DataOra = now.AddDays(10).Date.AddHours(9),
                DurataMinuti = 30,
                Stato = StatoAppuntamento.Programmato,
                MotivoVisita = "Elettrocardiogramma",
                DataCreazione = now
            }
        };

        await context.Appuntamenti.AddRangeAsync(appuntamenti);
        await context.SaveChangesAsync();
        Console.WriteLine($"✓ Creati {appuntamenti.Count} appuntamenti");

        // 6. REFERTI (per appuntamenti completati)
        var referti = new List<Referto>
        {
            new Referto
            {
                AppuntamentoId = 1, // Appuntamento Mario Rossi con Dr. Caruso
                PazienteId = 1,
                MedicoId = 1,
                Titolo = "Referto Visita Cardiologica",
                Descrizione = "Il paziente si è presentato per controllo della pressione arteriosa. Esame obiettivo nella norma.",
                Diagnosi = "Pressione arteriosa nei limiti. Ritmo cardiaco regolare.",
                Terapia = "Continuare terapia antipertensiva in corso (Ramipril 5mg 1cp/die). Controllo pressione domiciliare. Prossimo controllo tra 3 mesi.",
                DataCreazione = now.AddDays(-10)
            },
            new Referto
            {
                AppuntamentoId = 2, // Appuntamento Giulia Bianchi con Dr.ssa Martini
                PazienteId = 2,
                MedicoId = 2,
                Titolo = "Referto Visita Dermatologica",
                Descrizione = "Controllo mappatura nei. Eseguita dermatoscopia digitale su 15 lesioni pigmentate.",
                Diagnosi = "Nevi melanocitici benigni. Nessuna lesione sospetta.",
                Terapia = "Nessuna terapia necessaria. Protezione solare SPF 50+. Prossimo controllo tra 6 mesi.",
                DataCreazione = now.AddDays(-8)
            },
            new Referto
            {
                AppuntamentoId = 3, // Appuntamento Luca Verdi con Dr. Colombo
                PazienteId = 3,
                MedicoId = 3,
                Titolo = "Referto Visita Ortopedica",
                Descrizione = "Paziente riferisce dolore al ginocchio destro da circa 2 settimane. Dolore aumenta con attività fisica.",
                Diagnosi = "Sospetta lesione meniscale. Lieve versamento articolare.",
                Terapia = "Prescritta RMN ginocchio dx. FANS al bisogno (Ibuprofene 600mg). Riposo sportivo per 2 settimane. Rivalutazione post-RMN.",
                DataCreazione = now.AddDays(-5)
            }
        };

        await context.Referti.AddRangeAsync(referti);
        await context.SaveChangesAsync();
        Console.WriteLine($"✓ Creati {referti.Count} referti");

        Console.WriteLine("✅ Seeding completato con successo!");
        Console.WriteLine();
        Console.WriteLine("═══════════════════════════════════════════════════════════");
        Console.WriteLine("📋 CREDENZIALI DI ACCESSO PER TESTING");
        Console.WriteLine("═══════════════════════════════════════════════════════════");
        Console.WriteLine();
        Console.WriteLine("👤 ADMIN:");
        Console.WriteLine("   Username: admin");
        Console.WriteLine("   Password: Admin123!");
        Console.WriteLine();
        Console.WriteLine("📞 RECEPTIONIST:");
        Console.WriteLine("   Username: receptionist");
        Console.WriteLine("   Password: Recep123!");
        Console.WriteLine();
        Console.WriteLine("👨‍⚕️ MEDICI:");
        Console.WriteLine("   Username: dr.caruso    | Password: Cardio123!  | Specializzazione: Cardiologia");
        Console.WriteLine("   Username: dr.martini   | Password: Derma123!   | Specializzazione: Dermatologia");
        Console.WriteLine("   Username: dr.colombo   | Password: Orto123!    | Specializzazione: Ortopedia");
        Console.WriteLine("   Username: dr.romano    | Password: Pedia123!   | Specializzazione: Pediatria");
        Console.WriteLine("   Username: dr.esposito  | Password: Medgen123!  | Specializzazione: Medicina Generale");
        Console.WriteLine();
        Console.WriteLine("👥 PAZIENTI:");
        Console.WriteLine("   Username: mario.rossi       | Password: Mario123!");
        Console.WriteLine("   Username: giulia.bianchi    | Password: Giulia123!");
        Console.WriteLine("   Username: luca.verdi        | Password: Luca123!");
        Console.WriteLine("   Username: francesca.ferrari | Password: Francesca123!");
        Console.WriteLine("   Username: alessandro.russo  | Password: Alessandro123!");
        Console.WriteLine();
        Console.WriteLine("═══════════════════════════════════════════════════════════");
        Console.WriteLine("📊 DATI CREATI:");
        Console.WriteLine($"   • {pazienti.Count} Pazienti");
        Console.WriteLine($"   • {medici.Count} Medici");
        Console.WriteLine($"   • 5 Specializzazioni");
        Console.WriteLine($"   • {disponibilita.Count} Disponibilità medici");
        Console.WriteLine($"   • {utenti.Count} Utenti (tutti i ruoli)");
        Console.WriteLine($"   • {appuntamenti.Count} Appuntamenti (vari stati)");
        Console.WriteLine($"   • {referti.Count} Referti medici");
        Console.WriteLine("═══════════════════════════════════════════════════════════");
    }

    private static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }
}
