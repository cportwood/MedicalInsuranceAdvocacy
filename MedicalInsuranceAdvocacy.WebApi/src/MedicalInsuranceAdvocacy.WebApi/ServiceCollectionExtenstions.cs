using MedicalInsuranceAdvocacy.Service.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Exl.WebApi.Aspen
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
