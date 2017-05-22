using Warehouse.Data;

namespace Warehouse.Models.Tanks
{
    public class TankViewModel
    {
        public int Id { get; set; }

        public int Number { get; set; }

        public string Name { get; set; }

        public double CurrentLoad { get; set; }

        public double MaxCapacity { get; set; }

        public double CapacityLeft { get; set; }

        public Fertilizer Fertilizer { get; set; }

        public string CurrentFertilizer { get; set; }

    }
}