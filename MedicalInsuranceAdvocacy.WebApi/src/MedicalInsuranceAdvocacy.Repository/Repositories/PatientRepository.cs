using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using MedicalInsuranceAdvocacy.DataModel.Patient;
using MedicalInsuranceAdvocacy.RepoCore.DataContext;
using MedicalInsuranceAdvocacy.EfCore.Base;
using Microsoft.EntityFrameworkCore;

namespace MedicalInsuranceAdvocacy.Repository.Repositories
{
    public class PatientRepository : Repository<Patient>
    {
        public PatientRepository(IDataContextAsync context) 
            : base(context)
        {

        }

        public async Task<Patient> FindByIdAsync(int id)
        {

            var order = await Queryable().Where(o => o.PatientId == id).FirstOrDefaultAsync();
            return order;
        }

        public void Delete(int id)
        {
            var entity = FindByIdAsync(id);
            Delete(entity);
        }

        public async Task UpdateFrepIdStoredProc(int id, int frepId)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("Woid", id),
                new SqlParameter("Frepid", 22321)
            };

            await ExecuteStoredProcedureAsync("__UpdateFrep", parameters);
        }

        public async Task<Patient> GetPatientById(int patientId)
        {
            return await Queryable().Where(m => m.PatientId == patientId ).SingleOrDefaultAsync();
        }
        //public async Task<IEnumerable<Patient>> GetAllPatients()
        //{
        //    return await Queryable().Select()
        //}
        public async Task<DbDataReader> GetWoidToSyncByFrepIdStoredProc(int frepId, DateTime lastSyncDate)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("Frepid", frepId),
                new SqlParameter("LastSyncDate", lastSyncDate)
            };
            
           return await ExecuteStoredProcedureReaderAsync("esListWoidsToSyncForFrep", parameters);
        }

        //I commented this out. I will use this when we decide what data to return to the calling app once
        //the order(s) have been inserted into the database
        //public static List<WorkOrder> InsertOrders(this IRepository<WorkOrder> repository, List<WorkOrder> workorders)
        public void InsertOrder(Patient patient)
        {

            //Will need to add something here to return some data back to the calling app
            //example data like new assigned WOID(s), InsuredName, PolicyNumber, ProdlineID etc.
            //'m still thinking about this.

            //Below is just the generic of inserted the orders into the database
            //for now, in the future this will change as I figure out what I want to return back to the
            //calling client application after the orders have been inserted into the database tables.
            //foreach (var workorder in workorders)
            //{}

            this.Insert(patient);
            
            //return workorders;
        }


    }
}
