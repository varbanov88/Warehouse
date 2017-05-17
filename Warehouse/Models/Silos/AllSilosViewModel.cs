using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Warehouse.Models.Silos
{
    public class AllSilosViewModel
    {
        public int Id { get; set; }

        public int Number { get; set; }

        public string Name { get; set; }

        public double MaxCapacity { get; set; }

        public double CapacityLeft { get; set; }
    }
}