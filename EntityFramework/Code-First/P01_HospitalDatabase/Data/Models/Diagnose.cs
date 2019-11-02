namespace P01_HospitalDatabase.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Diagnose
    {
        public int DiagnoseId { get; set; }
        
        [MaxLength(MyValidator.DiagnoseNameLength)]
        public string Name { get; set; }

        [MaxLength(MyValidator.CommentsLength)]
        public string Comments { get; set; }

        public int PatientId { get; set; }

        public virtual Patient Patient { get; set; }
    }
}
