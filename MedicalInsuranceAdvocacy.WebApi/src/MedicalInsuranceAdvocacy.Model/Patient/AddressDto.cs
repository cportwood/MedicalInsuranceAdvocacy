using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalInsuranceAdvocacy.Model.Patient
{
    public class AddressDto
    {
       public int AddressId { get; set; }
       public string StreetName { get; set; }
       public string City { get; set; }
       public string State { get; set; }
       public string Country { get; set; }
       public string Zip { get; set; }
       public PatientDto Patient { get; set; }
    }
}
