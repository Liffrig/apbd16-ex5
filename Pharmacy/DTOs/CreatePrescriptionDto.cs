namespace Pharmacy.DTOs;
using System.ComponentModel.DataAnnotations;

public class CreatePrescriptionDto {
    
    [Required]
    public PatientDto Patient { get; set; }
        
    [Required]
    public DateTime Date { get; set; }
        
    [Required]
    public DateTime DueDate { get; set; }
        
    [Required]
    public int IdDoctor { get; set; }
        
    [Required]
    [MinLength(1)]
    [MaxLength(10)]
    public List<MedicamentDto> Medicaments { get; set; }
    
}