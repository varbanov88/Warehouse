using System.ComponentModel.DataAnnotations;

namespace YaraTask.Models.Silos
{
    public class CreateSiloModel
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [Range(50, 5000)]
        [Display(Name = "Max Capacity")]
        public double MaxCapacity { get; set; }

        [Required]
        [Range(1, 1000)]
        [Display(Name = "Silo Number")]
        public int SiloNumber { get; set; }

        [Display(Name = "Commodity")]
        public string CurrentCommodity { get; set; }
    }
}