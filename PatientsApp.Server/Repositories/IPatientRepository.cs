using PatientsApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PatientsApp.Repositories
{
    public interface IPatientRepository
    {
        Task<IEnumerable<Patient>> GetAllPatients(string search, int page, int pageSize);
        Task<Patient?> GetPatientById(int id);
        Task<Patient> CreatePatient(Patient patient);
        Task<bool> UpdatePatient(Patient patient);
        Task<bool> DeletePatient(int id);
        Task<int> GetTotalPatientsCount(string search);
    }
}

