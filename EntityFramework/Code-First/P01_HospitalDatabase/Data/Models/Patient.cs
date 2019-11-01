namespace P01_HospitalDatabase.Data.Models
{
    using P01_HospitalDatabase.Data.Models.Validations;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Patient
    {
        public int PatientId { get; set; }

        [MaxLength(Validation.FirstNameLength)]
        public string FirstName{ get; set; }

        [MaxLength(Validation.LastNameLength)]
        public string LastName { get; set; }

        [MaxLength(Validation.AddressLength)]
        public string Address { get; set; }

        [MaxLength(Validation.EmailLength)]
        public string Email { get; set; }

        public bool HasInsurance { get; set; }

        public virtual ICollection<PatientMedicament> Prescriptions { get; set; } = new HashSet<PatientMedicament>();

        public virtual ICollection<Visitation> Visitations { get; set; } = new HashSet<Visitation>();

        public virtual ICollection<Diagnose> Diagnoses { get; set; } = new HashSet<Diagnose>();
    }
}
