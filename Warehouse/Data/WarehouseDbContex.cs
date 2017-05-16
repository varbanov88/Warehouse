﻿using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using Warehouse.Models;
using YaraTask.Data;

namespace Warehouse.Data
{
    public class WarehouseDbContext : IdentityDbContext<User>
    {
        public WarehouseDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public virtual IDbSet<Silo> Silos { get; set; }

        public static WarehouseDbContext Create()
        {
            return new WarehouseDbContext();
        }
    }
}