using System;

namespace Warehouse.Data
{
    public class TankOperation
    {
        public TankOperation()
        {
            this.ActionDate = DateTime.Now;
        }

        public int Id { get; set; }

        public DateTime ActionDate { get; set; }

        public double AmountBeforeAction { get; set; }

        public string OperationName { get; set; }

        public double ActionAmount { get; set; }

        public double AmountAfterAction { get; set; }

        public int TankId { get; set; }

        public virtual Tank Tank { get; set; }

        public string OperatorName { get; set; }

        public string FertilizerName { get; set; }
    }
}