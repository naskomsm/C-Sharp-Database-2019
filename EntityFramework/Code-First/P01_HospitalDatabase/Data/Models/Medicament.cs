﻿namespace P01_HospitalDatabase.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Medicament
    {
        public int MedicamentId { get; set; }

        [MaxLength(MyValidator.MedicamentNameLength)]
        public string Name { get; set; }

        public virtual ICollection<PatientMedicament> Prescriptions { get; set; } = new HashSet<PatientMedicament>();
    }
}
