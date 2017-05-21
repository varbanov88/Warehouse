namespace Warehouse.Models.Silos
{
    public class DeleteSiloModel
    {
        public int Id { get; set; }

        public int Number { get; set; }

        public string Name { get; set; }

        public double CurrentLoad { get; set; }

        public double MaxCapacity { get; set; }

        public double CapacityLeft { get; set; }

        public string CurrentCommodity { get; set; }

    }
}