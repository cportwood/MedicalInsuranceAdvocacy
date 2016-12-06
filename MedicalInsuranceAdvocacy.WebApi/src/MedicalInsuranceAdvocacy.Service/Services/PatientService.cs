using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MedicalInsuranceAdvocacy.DataModel.Patient;
using MedicalInsuranceAdvocacy.Repository.Repositories;
using MedicalInsuranceAdvocacy.DbContext.DbContexts;

namespace MedicalInsuranceAdvocacy.Service.Services
{
    public interface IPatientService
    {
        Task<Patient> GetPatientById(int patientId);

      
    }
    public class PatientService : IPatientService
    {
        private readonly ApplicationContext _applicationContext;
        private readonly IMapper _mapper;
        private readonly PatientRepository _patientRepository;

        public PatientService(ApplicationContext applicationContext, IMapper mapper)
        {
            _applicationContext = applicationContext;
            _patientRepository = new PatientRepository(_applicationContext);
            _mapper = mapper;
        }
        public async Task<Patient> GetPatientById(int patientId)
        {
                return await _patientRepository.GetPatientById(patientId);
        }

      
    }
}
