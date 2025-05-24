using Microsoft.EntityFrameworkCore;
using Pharmacy.Data;
using Pharmacy.DTOs;
using Pharmacy.Models;
using Pharmacy.Exceptions;

namespace Pharmacy.Services;

public class PrescriptionService : IPrescriptionService {
    private readonly DatabaseContext _context;

    public PrescriptionService(DatabaseContext context) {
        _context = context;
    }

    public async Task<int> CreatePrescriptionAsync(CreatePrescriptionDto dto) {
        if (dto.DueDate < dto.Date) throw new ValidationException("DueDate must be greater than or equal to Date");
        if (dto.Medicaments.Count > 10)
            throw new ValidationException("Prescription cannot contain more than 10 medicaments");

        using var transaction = await _context.Database.BeginTransactionAsync();

        try {
            var medicamentIds = dto.Medicaments.Select(m => m.IdMedicament).ToList();
            var existingMedicaments = await _context.Medicaments
                .Where(m => medicamentIds.Contains(m.IdMedicament))
                .ToListAsync();

            if (existingMedicaments.Count != medicamentIds.Count) {
                var missingIds = medicamentIds.Except(existingMedicaments.Select(m => m.IdMedicament));
                throw new NotFoundException($"Medicaments with IDs {string.Join(", ", missingIds)} not found");
            }

            var doctor = await _context.Doctors.FindAsync(dto.IdDoctor);
            if (doctor == null) throw new NotFoundException($"Doctor with ID {dto.IdDoctor} not found");


            // TODO zribić kontruktory DTO
            Patient patient;
            if (dto.Patient.IdPatient.HasValue) {
                patient = await _context.Patients.FindAsync(dto.Patient.IdPatient.Value);
                if (patient == null) throw new NotFoundException($"Patient with ID {dto.Patient.IdPatient} not found");
            }
            else {
                patient = new Patient {
                    FirstName = dto.Patient.FirstName,
                    LastName = dto.Patient.LastName,
                    Birthdate = dto.Patient.Birthdate
                };
                _context.Patients.Add(patient);
                await _context.SaveChangesAsync();
            }

            var prescription = new Prescription {
                Date = dto.Date,
                DueDate = dto.DueDate,
                IdPatient = patient.IdPatient,
                IdDoctor = dto.IdDoctor
            };

            _context.Prescriptions.Add(prescription);
            await _context.SaveChangesAsync();


            foreach (var medicamentDto in dto.Medicaments) {
                var prescriptionMedicament = new PrescriptionMedicament {
                    IdPrescription = prescription.IdPrescription,
                    IdMedicament = medicamentDto.IdMedicament,
                    Dose = medicamentDto.Dose,
                    Details = medicamentDto.Details
                };
                _context.PrescriptionMedicaments.Add(prescriptionMedicament);
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return prescription.IdPrescription;
        }
        catch {
            await transaction.RollbackAsync();
            throw; // sprawdzić co zrobić
        }
    }

    public async Task<PatientDetailsDto> GetPatientDetailsAsync(int patientId) {
        var patient = await _context.Patients
            .Include(p => p.Prescriptions)
            .ThenInclude(pr => pr.Doctor)
            .Include(p => p.Prescriptions)
            .ThenInclude(pr => pr.PrescriptionMedicaments)
            .ThenInclude(pm => pm.Medicament)
            .FirstOrDefaultAsync(p => p.IdPatient == patientId);

        if (patient == null) throw new NotFoundException($"Patient with ID {patientId} not found");

        var result = new PatientDetailsDto {
            Patient = new PatientDto {
                IdPatient = patient.IdPatient,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                Birthdate = patient.Birthdate
            },

            Prescriptions = patient.Prescriptions
                .OrderBy(p => p.DueDate)
                .Select(p => new PrescriptionDetailsDto {
                    IdPrescription = p.IdPrescription,
                    Date = p.Date,
                    DueDate = p.DueDate,
                    Doctor = new DoctorDto {
                        IdDoctor = p.Doctor.IdDoctor,
                        FirstName = p.Doctor.FirstName,
                        LastName = p.Doctor.LastName,
                        Email = p.Doctor.Email
                    },
                    Medicaments = p.PrescriptionMedicaments.Select(pm => new MedicamentDetailsDto {
                        IdMedicament = pm.Medicament.IdMedicament,
                        Name = pm.Medicament.Name,
                        Description = pm.Medicament.Description,
                        Type = pm.Medicament.Type,
                        Dose = pm.Dose,
                        Details = pm.Details
                    }).ToList()
                }).ToList()
        };

        return result;
    }
}