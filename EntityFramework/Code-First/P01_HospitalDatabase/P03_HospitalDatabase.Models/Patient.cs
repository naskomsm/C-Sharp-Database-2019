namespace P01_HospitalDatabase.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Patient
    {
        public int PatientId { get; set; }

        [MaxLength(MyValidator.FirstNameLength)]
        public string FirstName { get; set; }

        [MaxLength(MyValidator.LastNameLength)]
        public string LastName { get; set; }

        [MaxLength(MyValidator.AddressLength)]
        public string Address { get; set; }

        [MaxLength(MyValidator.EmailLength)]
        public string Email { get; set; }

        public bool HasInsurance { get; set; }

        public ICollection<Visitation> Visitations { get; set; } = new HashSet<Visitation>();

        public ICollection<Diagnose> Diagnoses { get; set; } = new HashSet<Diagnose>();

        public ICollection<PatientMedicament> Prescriptions { get; set; } = new HashSet<PatientMedicament>();
    }
}
