using System;
using System.ComponentModel.DataAnnotations;

namespace Warehouse.Data
{
    public class Fertilizer
    {
        private string name;

        private double amount;

        public Fertilizer(string name, double amount)
        {
            this.Name = name;
            this.Amount = amount;
        }

        [Required]
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
                    throw new ArgumentException("Fertilizer name must be between 3 and 200 symbols");
                }

                this.name = value;
            }
        }

        [Required]
        public double Amount
        {
            get
            {
                return this.amount;
            }

            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Amount must be must be more than 0");
                }

                this.amount = value;
            }
        }
    }
}