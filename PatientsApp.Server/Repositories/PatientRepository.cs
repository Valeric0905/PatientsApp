using Microsoft.EntityFrameworkCore;
using PatientsApp.Data;
using PatientsApp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientsApp.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly AppDbContext _context;

        public PatientRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Patient>> GetAllPatients(string search, int page, int pageSize)
        {
            var query = _context.Patients.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(p =>
                    p.Name.Contains(search) ||
                    p.PhoneNumber.Contains(search) ||
                    p.Address.Contains(search));
            }

            return await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetTotalPatientsCount(string search)
        {
            var query = _context.Patients.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(p =>
                    p.Name.Contains(search) ||
                    p.PhoneNumber.Contains(search) ||
                    p.Address.Contains(search));
            }

            return await query.CountAsync();
        }

        public async Task<Patient?> GetPatientById(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            return patient;
        }


        public async Task<Patient> CreatePatient(Patient patient)
        {
            await _context.Patients.AddAsync(patient);
            await _context.SaveChangesAsync();
            return patient;
        }

        public async Task<bool> UpdatePatient(Patient patient)
        {
            var existingPatient = await _context.Patients.FindAsync(patient.Id);
            if (existingPatient == null)
                return false;

            existingPatient.Name = patient.Name;
            existingPatient.Gender = patient.Gender;
            existingPatient.DateOfBirth = patient.DateOfBirth;
            existingPatient.PhoneNumber = patient.PhoneNumber;
            existingPatient.Address = patient.Address;

            _context.Patients.Update(existingPatient);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeletePatient(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
                return false;

            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
