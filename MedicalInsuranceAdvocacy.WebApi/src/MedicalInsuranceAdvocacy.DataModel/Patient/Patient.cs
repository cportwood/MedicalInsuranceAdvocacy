using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedicalInsuranceAdvocacy.EfCore.Base;

namespace MedicalInsuranceAdvocacy.DataModel.Patient
{
    public class Patient : Entity
    {
       public int PatientId { get; set; }
       public string FirstName { get; set; }
       public string LastName { get; set; }
       public string Password { get; set;}
       public string Email { get; set; }
       public List<Address> Addresses { get; set; }
                       

    }
}
