using System;

namespace Warehouse.Models.Silos
{
    public class AllOperationsModel
    {
        public int Id { get; set; }

        public DateTime ActionDate { get; set; }

        public double AmountBeforeAction { get; set; }

        public string OperationName { get; set; }

        public double ActionAmount { get; set; }

        public double AmountAfterAction { get; set; }

        public int SiloId { get; set; }
    }
}