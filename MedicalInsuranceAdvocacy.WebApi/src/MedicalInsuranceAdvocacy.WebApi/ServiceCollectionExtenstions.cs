using MedicalInsuranceAdvocacy.Service.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MedicalInsuranceAdvocacy.WebApi
{
    public static class ServiceCollectionExtenstions
    {
        public static IServiceCollection RegisterServices(
      this IServiceCollection services)
        {
          
           
            services.AddTransient<IPatientService, PatientService>();
           return services;
        }
    }
}
