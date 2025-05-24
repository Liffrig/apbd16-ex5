using System.ComponentModel.DataAnnotations;

namespace Pharmacy.DTOs;

public class MedicamentDto {
    [Required]
    public int IdMedicament { get; set; }
        
    [Range(1, int.MaxValue)]
    public int Dose { get; set; }
        
    public string Details { get; set; }
}