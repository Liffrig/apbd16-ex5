using Microsoft.AspNetCore.Mvc;
using Pharmacy.DTOs;
using Pharmacy.Services;

namespace Pharmacy.Controllers;

public class PharmacyController : ControllerBase {
    
    private readonly IPrescriptionService _prescriptionService;
            
    public PharmacyController(IPrescriptionService prescriptionService) {
        _prescriptionService = prescriptionService;
    }
            
    [HttpPost]
    public async Task<IActionResult> CreatePrescription([FromBody] CreatePrescriptionDto dto) {
        try {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
                    
            var prescriptionId = await _prescriptionService.CreatePrescriptionAsync(dto);
            return CreatedAtAction(nameof(CreatePrescription), new { id = prescriptionId }, prescriptionId);
        }
                
        catch (Exception ex) {
            return StatusCode(500, "An erroroccurred");
        }
    }
            
    [HttpGet("patient/{patientId}")]
    public async Task<IActionResult> GetPatientDetails(int patientId) {
        try {
            var patientDetails = await _prescriptionService.GetPatientDetailsAsync(patientId);
            return Ok(patientDetails);
        }
             
        catch (Exception ex) {
            return StatusCode(500, "An error occurred");
        }
    }
    
}