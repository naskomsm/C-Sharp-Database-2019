namespace P01_HospitalDatabase.Data.Models
{
    using P01_HospitalDatabase.Data.Models.Validations;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Doctor
    {
        public int DoctorId { get; set; }

        [MaxLength(Validation.DoctorNameLength)]
        public string Name { get; set; }

        [MaxLength(Validation.DoctorSpecialityLength)]
        public string Specialty { get; set; }

        public virtual ICollection<Visitation> Visitations { get; set; } = new HashSet<Visitation>();
    }
}
