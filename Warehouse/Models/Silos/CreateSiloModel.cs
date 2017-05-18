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
        public double MaxCapacity { get; set; }

        [Required]
        [Range(1, 1000)]
        public int SiloNumber { get; set; }
    }
}