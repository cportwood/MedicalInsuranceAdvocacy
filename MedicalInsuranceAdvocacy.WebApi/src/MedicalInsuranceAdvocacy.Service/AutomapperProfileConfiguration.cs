using AutoMapper;
using MedicalInsuranceAdvocacy.DataModel.Patient;
using MedicalInsuranceAdvocacy.Model.Patient;

namespace MedicalInsuranceAdvocacy.Service
{

    public class AutoMapperProfileConfiguration : Profile
        {
            protected override void Configure()
            {
                CreateMap<Patient, PatientDto>().ReverseMap();
                CreateMap<Address, AddressDto>().ReverseMap();
             }
        }
    
}
