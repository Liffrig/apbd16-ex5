using System.ComponentModel.DataAnnotations;

namespace Pharmacy.DTOs;

public class PatientDto {
    public int? IdPatient { get; set; }
        
    [Required]
    public string FirstName { get; set; }
        
    [Required]
    public string LastName { get; set; }
        
    [Required]
    public DateTime Birthdate { get; set; }
}