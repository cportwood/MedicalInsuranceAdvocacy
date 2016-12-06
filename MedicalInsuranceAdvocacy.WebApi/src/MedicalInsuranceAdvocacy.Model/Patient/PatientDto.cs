using System.Collections.Generic;

namespace MedicalInsuranceAdvocacy.Model.Patient
{
    public class PatientDto
    {
        public int PatientId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public List<AddressDto> Addresses { get; set; }
    }
}
