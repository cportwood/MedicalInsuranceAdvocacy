using System.Collections.Generic;
using MedicalInsuranceAdvocacy.DataModel.Patient;
using MedicalInsuranceAdvocacy.DbContext.DbContexts;

namespace MedicalInsuranceAdvocacy.DbContext
{
    public static class DatabaseSeeder
    {
        public static void SeedPatientData(ApplicationContext context)
        {
            var patient1 = new Patient
            {
                PatientId = 1,
                FirstName = "Samuel",
                LastName = "Welckin",
                Email = "sam.wel@gmail.com",
                Addresses =
                    new List<Address>
                    {
                        new Address
                        {
                            AddressId = 1,
                            City = "OverlanPark",
                            Country = "USA",
                            Zip = "66212",
                            State = "KS",
                            StreetName = "6085 South Remin St"
                        }
                    }
            };
            context.Patients.Add(patient1);
            var patient2 = new Patient
            {
                PatientId = 2,
                FirstName = "Philips",
                LastName = "Faithniyr",
                Email = "faith.phil@gmail.com",
                Addresses =
                   new List<Address>
                   {
                        new Address
                        {
                            AddressId = 3,
                            City = "Kansas City",
                            Country = "USA",
                            Zip = "66235",
                            State = "KS",
                            StreetName = "455 E Patron Drive"
                        }
                   }
            };
            context.Patients.Add(patient2);
            context.SaveChanges();
        }
    }
}
