﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Warehouse.Models;

namespace Warehouse.Data
{
    public class Tank
    {
        private string name;
        private int number;
        private double maxCapacity;
        
        public Tank()
        {
            this.Operations = new List<TankOperation>();
        }

        public Tank(string name, int number, double maxCapacity)
            : this()
        {
            this.Name = name;
            this.Number = number;
            this.MaxCapacity = maxCapacity;
        }

        public int Id { get; set; }

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
                    throw new ArgumentException("Tanker Name must be between 3 and 200 symbols");
                }

                this.name = value;
            }
        }

        [Required]
        public int Number
        {
            get
            {
                return this.number;
            }

            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Tanker number must be greater than 0");
                }

                this.number = value;
            }
        }

        [Required]
        public double MaxCapacity
        {
            get
            {
                return this.maxCapacity;
            }

            set
            {
                if (value < 50)
                {
                    throw new ArgumentException("Tanker capacity cannot be less than 50 liters");
                }

                this.maxCapacity = value;
            }
        }

        public double CurrentLoad { get; set; }

        public string CurrentFertilizer { get; set; }

        public List<TankOperation> Operations { get; set; }

        public string TankCreatorId { get; set; }

        public virtual User Creator { get; set; }

        public void AddFertilizer(Fertilizer fertilizer, string name)
        {
            if (fertilizer.Amount + this.CurrentLoad > this.MaxCapacity)
            {
                var availabaleCapacity = this.MaxCapacity - this.CurrentLoad;
                throw new ArgumentException($"You can add up to {availabaleCapacity} liters");
            }

            if (this.CurrentFertilizer != null && this.CurrentFertilizer != fertilizer.Name.ToLower())
            {
                throw new ArgumentException($"You can add only {this.CurrentFertilizer} in this tanker");
            }

            if (this.CurrentFertilizer == null)
            {
                this.CurrentFertilizer = fertilizer.Name.ToLower();
            }

            var operation = new TankOperation
            {
                ActionAmount = fertilizer.Amount,
                AmountAfterAction = this.CurrentLoad + fertilizer.Amount,
                AmountBeforeAction = this.CurrentLoad,
                OperationName = "Import",
                TankId = this.Id,
                OperatorName = name,
                FertilizerName = fertilizer.Name.ToLower()
            };

            this.Operations.Add(operation);

            this.CurrentLoad += fertilizer.Amount;
        }
    }
}