using System.ComponentModel.DataAnnotations;

namespace Warehouse.Models.Tanks
{
    public class DeleteTankModel
    {
        public int Id { get; set; }

        public int Number { get; set; }

        public string Name { get; set; }

        [Display(Name = "Max Capacity")]
        public double MaxCapacity { get; set; }

        [Display(Name = "Current Fertilizer")]
        public string CurrentFertilizer { get; set; }

        [Display(Name = "Current Load")]
        public double CurrentLoad { get; set; }

        [Display(Name = "Capacity Left")]
        public double CapacityLeft { get; set; }
    }
}