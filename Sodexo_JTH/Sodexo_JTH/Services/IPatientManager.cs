using Sodexo_JTH.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Sodexo_JTH.Services
{
    public interface IPatientManager
    {
        Task<ObservableCollection<mstr_patient_info>> GetPatientsByWardBed(string DateFormat, int wardID, int bedID);
    }
}