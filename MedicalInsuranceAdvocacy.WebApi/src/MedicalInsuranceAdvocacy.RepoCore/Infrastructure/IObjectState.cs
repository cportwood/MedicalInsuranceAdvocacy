using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalInsuranceAdvocacy.RepoCore.Infrastructure
{
    public interface IObjectState
    {
        [NotMapped]
        ObjectState ObjectState { get; set; }
    }
}
