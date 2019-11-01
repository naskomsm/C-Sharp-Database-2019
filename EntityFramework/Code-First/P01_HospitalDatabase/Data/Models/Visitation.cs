namespace P01_HospitalDatabase.Data.Models
{
    using P01_HospitalDatabase.Data.Models.Validations;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Visitation
    {
        public int VisitationId { get; set; }

        public DateTime Date { get; set; }

        [MaxLength(Validation.CommentsLength)]
        public string Comments { get; set; }

        public int PatientId { get; set; }

        public virtual Patient Patient { get; set; }

        public int DoctorId { get; set; }

        public virtual Doctor Doctor { get; set; }
    }
}
