using System.ComponentModel.DataAnnotations;

namespace Warehouse.Models.Silos
{
    public class AddCommodityModel
    {
        [Required(ErrorMessage = "You must enter commodity name")]
        [Display(Name = "Commodity Name")]
        public string CommodityName { get; set; }

        [Required]
        public double Amount { get; set; }
    }
}