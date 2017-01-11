using System.Collections.Generic;
using System.Threading.Tasks;
using MedicalInsuranceAdvocacy.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace MedicalInsuranceAdvocacy.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class PatientController : Controller
    {
        private readonly IPatientService _patientService;

        public PatientController([FromServices]IPatientService patientService)
        {
            _patientService = patientService;
        }
        // GET api/patient
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/patient/5
        [HttpGet("GetPatientById")]
        public async Task<IActionResult> GetPatientById(int patientId)
        {
            var result = await _patientService.GetPatientById(patientId);
            if (result != null) return new OkObjectResult(result); else return new BadRequestResult();

        }

        // POST api/patient
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/patient/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/patient/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
