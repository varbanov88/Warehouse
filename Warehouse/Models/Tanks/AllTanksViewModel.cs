namespace Warehouse.Models.Tanks
{
    public class AllTanksViewModel
    {
        public int Id { get; set; }

        public int Number { get; set; }

        public string Name { get; set; }

        public string CurrentFertilizer { get; set; }

        public double MaxCapacity { get; set; }

        public double CapacityLeft { get; set; }

    }
}