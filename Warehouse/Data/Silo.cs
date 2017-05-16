using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Warehouse.Models;

namespace YaraTask.Data
{
    public class Silo
    {
        private string name;

        private string commodity;

        private double maxCapacity;

        private double currentLoad;

        private int siloNumber;

        public Silo(string name, double maxCapacity, int siloNumber)
        {
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

        public string SiloCreatorId { get; set; } 

        public virtual User Creator { get; set; }
    }
}