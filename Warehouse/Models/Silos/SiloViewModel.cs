using Warehouse.Data;

namespace Warehouse.Models.Silos
{
    public class SiloViewModel
    {
        public int Id { get; set; }

        public int Number { get; set; }

        public string Name { get; set; }

        public double CurrentLoad { get; set; }

        public double MaxCapacity { get; set; }

        public double CapacityLeft { get; set; }

        public Commodity Commodity { get; set; }
    }
}