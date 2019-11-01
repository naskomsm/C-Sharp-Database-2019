namespace P01_HospitalDatabase.Data.Models
{
    using P01_HospitalDatabase.Data.Models.Validations;
    using System.ComponentModel.DataAnnotations;

    public class Diagnose
    {
        public int DiagnoseId { get; set; }
        
        [MaxLength(Validation.DiagnoseNameLength)]
        public string Name { get; set; }

        [MaxLength(Validation.CommentsLength)]
        public string Comments { get; set; }

        public int PatientId { get; set; }

        public virtual Patient Patient { get; set; }
    }
}
