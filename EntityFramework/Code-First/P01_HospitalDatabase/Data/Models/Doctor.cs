namespace P01_HospitalDatabase.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Doctor
    {
        public int DoctorId { get; set; }

        [MaxLength(MyValidator.DoctorNameLength)]
        public string Name { get; set; }

        [MaxLength(MyValidator.DoctorSpecialityLength)]
        public string Specialty { get; set; }

        public virtual ICollection<Visitation> Visitations { get; set; } = new HashSet<Visitation>();
    }
}
