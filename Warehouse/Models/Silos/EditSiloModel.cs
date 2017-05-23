using System.ComponentModel.DataAnnotations;

namespace Warehouse.Models.Silos
{
    public class EditSiloModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Silo must have name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Silo must have number")]
        public int Number { get; set; }

        [Required(ErrorMessage = "You must define silo capacity")]
        [Display(Name = "Max Capacity")]
        public double MaxCapacity { get; set; }
    }
}