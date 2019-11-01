namespace P01_HospitalDatabase.Data.Models
{
    using P01_HospitalDatabase.Data.Models.Validations;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Medicament
    {
        public int MedicamentId { get; set; }

        [MaxLength(Validation.MedicamentNameLength)]
        public string Name { get; set; }

        public virtual ICollection<PatientMedicament> Prescriptions { get; set; } = new HashSet<PatientMedicament>();
    }
}
