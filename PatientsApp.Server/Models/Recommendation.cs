namespace PatientsApp.Models
{
    public class Recommendation
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public string Details { get; set; }
        public bool IsCompleted { get; set; }
    }
}
