using Microsoft.EntityFrameworkCore;
using MedicalAppointments.Models;

namespace MedicalAppointments.Data;

/// <summary>
/// Database Context principale dell'applicazione
/// </summary>
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // DbSets
    public DbSet<Paziente> Pazienti { get; set; }
    public DbSet<Medico> Medici { get; set; }
    public DbSet<Specializzazione> Specializzazioni { get; set; }
    public DbSet<Appuntamento> Appuntamenti { get; set; }
    public DbSet<Referto> Referti { get; set; }
    public DbSet<DisponibilitaMedico> DisponibilitaMedici { get; set; }
    public DbSet<Utente> Utenti { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configurazione Paziente
        modelBuilder.Entity<Paziente>(entity =>
        {
            entity.HasIndex(e => e.CodiceFiscale).IsUnique();
            entity.HasIndex(e => e.Email).IsUnique();
        });

        // Configurazione Medico
        modelBuilder.Entity<Medico>(entity =>
        {
            entity.HasIndex(e => e.Email).IsUnique();
            entity.HasIndex(e => e.NumeroAlbo).IsUnique();

            entity.HasOne(m => m.Specializzazione)
                .WithMany(s => s.Medici)
                .HasForeignKey(m => m.SpecializzazioneId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // Configurazione Appuntamento
        modelBuilder.Entity<Appuntamento>(entity =>
        {
            entity.HasOne(a => a.Paziente)
                .WithMany(p => p.Appuntamenti)
                .HasForeignKey(a => a.PazienteId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(a => a.Medico)
                .WithMany(m => m.Appuntamenti)
                .HasForeignKey(a => a.MedicoId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(a => new { a.MedicoId, a.DataOra });
        });

        // Configurazione Referto
        modelBuilder.Entity<Referto>(entity =>
        {
            entity.HasOne(r => r.Appuntamento)
                .WithOne(a => a.Referto)
                .HasForeignKey<Referto>(r => r.AppuntamentoId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(r => r.Paziente)
                .WithMany(p => p.Referti)
                .HasForeignKey(r => r.PazienteId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(r => r.Medico)
                .WithMany(m => m.Referti)
                .HasForeignKey(r => r.MedicoId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Configurazione DisponibilitaMedico
        modelBuilder.Entity<DisponibilitaMedico>(entity =>
        {
            entity.HasOne(d => d.Medico)
                .WithMany(m => m.Disponibilita)
                .HasForeignKey(d => d.MedicoId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(d => new { d.MedicoId, d.GiornoSettimana });
        });

        // Configurazione Utente
        modelBuilder.Entity<Utente>(entity =>
        {
            entity.HasIndex(e => e.Username).IsUnique();
            entity.HasIndex(e => e.Email).IsUnique();

            entity.HasOne(u => u.Paziente)
                .WithMany()
                .HasForeignKey(u => u.PazienteId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(u => u.Medico)
                .WithMany()
                .HasForeignKey(u => u.MedicoId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // Seed data per Specializzazioni
        modelBuilder.Entity<Specializzazione>().HasData(
            new Specializzazione { Id = 1, Nome = "Cardiologia", Descrizione = "Specializzazione in malattie del cuore e del sistema cardiovascolare" },
            new Specializzazione { Id = 2, Nome = "Dermatologia", Descrizione = "Specializzazione in malattie della pelle" },
            new Specializzazione { Id = 3, Nome = "Ortopedia", Descrizione = "Specializzazione in malattie dell'apparato muscolo-scheletrico" },
            new Specializzazione { Id = 4, Nome = "Pediatria", Descrizione = "Specializzazione in medicina infantile" },
            new Specializzazione { Id = 5, Nome = "Medicina Generale", Descrizione = "Medicina di base" }
        );
    }
}
