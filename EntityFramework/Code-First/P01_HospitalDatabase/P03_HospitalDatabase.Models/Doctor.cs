namespace P01_HospitalDatabase.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Doctor
    {
        public int DoctorId { get; set; }

        [MaxLength(MyValidator.DoctorNameLength)]
        public string Name { get; set; }

        [MaxLength(MyValidator.SpecialtyNameLength)]
        public string Specialty { get; set; }

        public ICollection<Visitation> Visitations { get; set; } = new HashSet<Visitation>();
    }
}
