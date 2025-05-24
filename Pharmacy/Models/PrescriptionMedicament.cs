using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pharmacy.Models;

[Table("PrescriptionMedicament")]
[PrimaryKey(nameof(IdMedicament), nameof(IdPrescription))]
public class PrescriptionMedicament
{
    [Required]
    [ForeignKey(nameof(Medicament))]
    public int IdMedicament { get; set; }
    public virtual Medicament Medicament { get; set; }
    
    [Required]
    [ForeignKey(nameof(Prescription))]
    public int IdPrescription { get; set; }
    public virtual Prescription Prescription { get; set; }
    
    [Range(1, int.MaxValue)]
    public int Dose { get; set; }
        
    [MaxLength(500)]
    public string Details { get; set; }
}