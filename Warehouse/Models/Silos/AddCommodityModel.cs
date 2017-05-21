using System.ComponentModel.DataAnnotations;

namespace Warehouse.Models.Silos
{
    public class AddCommodityModel
    {
        [Required]
        public string CommodityName { get; set; }

        [Required]
        public double Amount { get; set; }
    }
}