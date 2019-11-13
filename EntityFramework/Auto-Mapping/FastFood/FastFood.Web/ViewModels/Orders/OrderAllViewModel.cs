namespace FastFood.Web.ViewModels.Orders
{
    using System.ComponentModel.DataAnnotations;

    public class OrderAllViewModel
    {
        public int OrderId { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 3)]
        public string Customer { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 3)]
        public string Employee { get; set; }

        public string DateTime { get; set; }
    }
}
