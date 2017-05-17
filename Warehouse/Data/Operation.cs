using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Warehouse.Data
{
    public class Operation
    {
        public Operation()
        {
            this.actionDate = DateTime.Now;
        }

        public DateTime actionDate;
        public double amountBeforeAction;
        public string operation;
        public double actionAmount;
        public double amountAfterAction;
    }
}