using System.ComponentModel.DataAnnotations;

namespace Warehouse.Models.Tanks
{
    public class EditTankModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tank must have name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Tank must have number")]
        public int Number { get; set; }

        [Required(ErrorMessage = "You must define tank capacity")]
        public double MaxCapacity { get; set; }
    }
}