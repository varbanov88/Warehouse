using System.ComponentModel.DataAnnotations;

namespace Warehouse.Models.Tankers
{
    public class CreateTankModel
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Max Capacity")]
        [Range(50, 5000)]
        public double MaxCapacity { get; set; }

        [Required]
        [Display(Name = "Tank Number")]
        [Range(1, 1000)]
        public int TankNumber { get; set; }

        [Display(Name = "Fertilizer")]
        public string CurrentFertilizer { get; set; }
    }
}