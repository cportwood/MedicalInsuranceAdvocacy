using System.ComponentModel.DataAnnotations.Schema;
using MedicalInsuranceAdvocacy.RepoCore.Infrastructure;

namespace MedicalInsuranceAdvocacy.EfCore.Base
{
    public class Entity : IObjectState
    {
        [NotMapped]
        public ObjectState ObjectState { get; set; }
    }
}
