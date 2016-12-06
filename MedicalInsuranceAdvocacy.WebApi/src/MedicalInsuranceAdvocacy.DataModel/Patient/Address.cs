using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedicalInsuranceAdvocacy.EfCore.Base;

namespace MedicalInsuranceAdvocacy.DataModel.Patient
{
    public class Address : Entity
    {
       public int AddressId { get; set; }
       public string StreetName { get; set; }
       public string City { get; set; }
       public string State { get; set; }
       public string Country { get; set; }
       public string Zip { get; set; }
       public Patient Patient { get; set; }
    }
}
