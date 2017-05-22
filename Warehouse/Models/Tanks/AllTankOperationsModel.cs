using System;

namespace Warehouse.Models.Tanks
{
    public class AllTankOperationsModel
    {
        public int Id { get; set; }

        public DateTime ActionDate { get; set; }

        public double AmountBeforeAction { get; set; }

        public string OperationName { get; set; }

        public double ActionAmount { get; set; }

        public double AmountAfterAction { get; set; }

        public int TankId { get; set; }

        public string Operator { get; set; }

        public string FertilizerName { get; set; }
    }
}