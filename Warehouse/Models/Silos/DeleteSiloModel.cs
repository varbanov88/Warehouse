using System.ComponentModel.DataAnnotations;

namespace Warehouse.Models.Silos
{
    public class DeleteSiloModel
    {
        public int Id { get; set; }

        public int Number { get; set; }

        public string Name { get; set; }

        [Display(Name = "Current Load")]
        public double CurrentLoad { get; set; }

        [Display(Name = "Max Capacity")]
        public double MaxCapacity { get; set; }

        [Display(Name = "Capacity Left")]
        public double CapacityLeft { get; set; }

        [Display(Name = "Current Commodity")]
        public string CurrentCommodity { get; set; }
    }
}