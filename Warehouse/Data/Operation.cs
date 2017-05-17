﻿using System;

namespace Warehouse.Data
{
    public class Operation
    {
        public Operation()
        {
            this.ActionDate = DateTime.Now;
        }

        public int Id { get; set; }

        public DateTime ActionDate { get; set; }

        public double AmountBeforeAction { get; set; }

        public string OperationName { get; set; }

        public double ActionAmount { get; set; }

        public double AmountAfterAction { get; set; }
    }
}