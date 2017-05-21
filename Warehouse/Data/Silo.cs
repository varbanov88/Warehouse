using System;
using System.Collections.Generic;
using Warehouse.Data;
using Warehouse.Models;
using Warehouse.Models.Silos;

namespace YaraTask.Data
{
    public class Silo
    {
        private string name;

        private double maxCapacity;

        private int siloNumber;


        public Silo()
        {
            this.Operations = new List<Operation>();
        }

        public Silo(string name, double maxCapacity, int siloNumber)
        {
            this.Operations = new List<Operation>();
            this.Name = name;
            this.MaxCapacity = maxCapacity;
            this.SiloNumber = siloNumber;
        }

        public int Id { get; set; }

        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                if (value.Length < 3 || value.Length > 200)
                {
                    throw new ArithmeticException("The length of the Name cannot be less than 3 symbols or more than 200 symbols");
                }

                this.name = value;
            }
        }

        public string CurrentCommodity { get; set; }

        public double CurrentLoad { get; set; }

        public double MaxCapacity
        {
            get
            {
                return this.maxCapacity;
            }
            set
            {
                if (value < 50 || value > 50000)
                {
                    throw new ArgumentException($"{nameof(MaxCapacity)} cannot be less than 50 tons or more than 50 000 tons!");
                }

                this.maxCapacity = value;
            }
        }

        public int SiloNumber
        {
            get
            {
                return this.siloNumber;
            }
            set
            {
                if (value < 0 )
                {
                    throw new ArgumentException("Silo number cannot be less than 0");
                }

                this.siloNumber = value;
            }
        }

        public List<Operation> Operations { get; set; }

        public string SiloCreatorId { get; set; } 

        public virtual User Creator { get; set; }

        public void AddCommodity(Commodity commodity, string name)
        {
            if (commodity.Amount + this.CurrentLoad > this.maxCapacity)
            {
                var availableSpace = this.maxCapacity - this.CurrentLoad;
                throw new ArgumentException($"Capacity not enough. You can import up to {availableSpace} tones");
            }

            if (commodity.Amount <= 0)
            {
                throw new ArgumentException("You must enter positive number");
            }

            if (this.CurrentCommodity == null)
            {
                this.CurrentCommodity = commodity.Name.ToLower();
            }

            if (!this.CurrentCommodity.Equals(commodity.Name.ToLower()))
            {
                throw new ArgumentException($"In this silo you can import only {this.CurrentCommodity}");
            }

            else
            {
                var operation = new Operation
                {
                    ActionAmount = commodity.Amount,
                    AmountAfterAction = this.CurrentLoad + commodity.Amount,
                    AmountBeforeAction = this.CurrentLoad,
                    OperationName = "Import",
                    SiloId = this.Id,
                    OperatorName = name,
                    CommodityName = commodity.Name.ToLower(),
                };

                this.Operations.Add(operation);

                this.CurrentLoad += commodity.Amount;
            }
        }

        public void ExportCommodity(Commodity commodity, string name)
        {
            if (this.CurrentLoad - commodity.Amount < 0)
            {
                var exportLimit = this.CurrentLoad;
                throw new ArgumentException($"You can export up to {exportLimit} tones");
            }

            if (commodity.Amount <= 0)
            {
                throw new ArgumentException("You must enter positive number");
            }

            if (!this.CurrentCommodity.Equals(commodity.Name.ToLower()))
            {
                throw new ArgumentException($"There is {this.CurrentCommodity} in this silo. You cannot export other commodity");
            }

            else
            {
                var operation = new Operation
                {
                    ActionAmount = commodity.Amount,
                    AmountAfterAction = this.CurrentLoad - commodity.Amount,
                    AmountBeforeAction = this.CurrentLoad,
                    OperationName = "Export",
                    SiloId = this.Id, 
                    OperatorName = name,
                    CommodityName = commodity.Name.ToLower()
                };

                this.Operations.Add(operation);
                this.CurrentLoad -= commodity.Amount;

                if (this.CurrentLoad == 0)
                {
                    this.CurrentCommodity = null;
                }
            }
        }

        public bool CanDeleteSilo(DeleteSiloModel silo)
        {
            if (silo.CurrentCommodity != null)
            {
                throw new ArithmeticException($"The Silo {silo.Name} cannot be deleted before it is empty");
            }

            return true;
        }

    }
}