using Pharmacy.DTOs;

namespace Pharmacy.Services;

public interface IPrescriptionService
{
    Task<int> CreatePrescriptionAsync(CreatePrescriptionDto dto);
    Task<PatientDetailsDto> GetPatientDetailsAsync(int patientId);
}