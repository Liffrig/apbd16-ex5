using Microsoft.EntityFrameworkCore;
using Pharmacy.Models;
namespace Pharmacy.Data;

public class DatabaseContext : DbContext {
    
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Medicament> Medicaments { get; set; }
    public DbSet<Prescription> Prescriptions { get; set; }
    public DbSet<PrescriptionMedicament> PrescriptionMedicaments { get; set; }
    
    
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        // SEED DATA - dane początkowe - do sprawdzić czy można bez tego
        SeedData(builder);
    }

    private void SeedData(ModelBuilder builder) {
        
        builder.Entity<Doctor>().HasData(
        new Doctor {
            IdDoctor = 1,
            FirstName = "Jan",
            LastName = "Kowalski",
            Email = "jan.kowalski@hospital.com"
        },
        new Doctor {
            IdDoctor = 2,
            FirstName = "Anna",
            LastName = "Nowak",
            Email = "anna.nowak@hospital.com"
        },
        new Doctor {
                IdDoctor = 3,
                FirstName = "Piotr",
                LastName = "Wiśniewski",
                Email = "piotr.wisniewski@hospital.com"
        });
        
        builder.Entity<Medicament>().HasData(
            new Medicament
            {
                IdMedicament = 1,
                Name = "Aspirin",
                Description = "Acetylsalicylic acid - pain reliever and anti-inflammatory",
                Type = "Tablet"
            },
            new Medicament
            {
                IdMedicament = 2,
                Name = "Ibuprofen",
                Description = "Non-steroidal anti-inflammatory drug (NSAID)",
                Type = "Tablet"
            },
            new Medicament
            {
                IdMedicament = 3,
                Name = "Paracetamol",
                Description = "Acetaminophen - pain reliever and fever reducer",
                Type = "Tablet"
            },
            new Medicament
            {
                IdMedicament = 4,
                Name = "Amoxicillin",
                Description = "Penicillin antibiotic used to treat bacterial infections",
                Type = "Capsule"
            },
            new Medicament
            {
                IdMedicament = 5,
                Name = "Omeprazole",
                Description = "Proton pump inhibitor for acid reflux and ulcers",
                Type = "Capsule"
            });
            
          
            builder.Entity<Patient>().HasData(
                new Patient
                {
                    IdPatient = 1,
                    FirstName = "Marek",
                    LastName = "Testowy",
                    Birthdate = new DateTime(1985, 3, 15)
                },
                new Patient
                {
                    IdPatient = 2,
                    FirstName = "Katarzyna",
                    LastName = "Przykładowa",
                    Birthdate = new DateTime(1992, 7, 22)
                }
            );
            
            builder.Entity<Prescription>().HasData(
                new Prescription
                {
                    IdPrescription = 1,
                    Date = new DateTime(2024, 1, 15, 10, 30, 0),
                    DueDate = new DateTime(2024, 2, 15, 10, 30, 0),
                    IdPatient = 1,
                    IdDoctor = 1
                },
                new Prescription
                {
                    IdPrescription = 2,
                    Date = new DateTime(2024, 1, 20, 14, 15, 0),
                    DueDate = new DateTime(2024, 2, 20, 14, 15, 0),
                    IdPatient = 2,
                    IdDoctor = 2
                }
            );
            
            builder.Entity<PrescriptionMedicament>().HasData(
                new PrescriptionMedicament
                {
                    IdPrescription = 1,
                    IdMedicament = 1,
                    Dose = 2,
                    Details = "Przyjmować 2 razy dziennie po posiłku"
                },
                new PrescriptionMedicament
                {
                    IdPrescription = 1,
                    IdMedicament = 3,
                    Dose = 1,
                    Details = "W razie potrzeby, maksymalnie 3 razy dziennie"
                },
                new PrescriptionMedicament
                {
                    IdPrescription = 2,
                    IdMedicament = 2,
                    Dose = 1,
                    Details = "Raz dziennie rano"
                },
                new PrescriptionMedicament
                {
                    IdPrescription = 2,
                    IdMedicament = 5,
                    Dose = 1,
                    Details = "Raz dziennie na czczo, 30 minut przed jedzeniem"
                }
            );
        }
}