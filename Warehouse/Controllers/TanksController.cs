using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Warehouse.Data;
using Warehouse.Models.Tanks;

namespace Warehouse.Controllers
{
    public class TanksController : Controller
    {
        public ActionResult All()
        {
            var db = new WarehouseDbContext();

            var tanks = db.Tanks
                .OrderBy(t => t.Number)
                .Select(t => new AllTanksViewModel
                {
                    Id = t.Id,
                    Number = t.Number,
                    Name = t.Name,
                    MaxCapacity = t.MaxCapacity,
                    CapacityLeft = t.MaxCapacity - t.CurrentLoad,
                    CurrentFertilizer = t.CurrentFertilizer
                })
                .ToList();

            foreach (var tank in tanks)
            {
                if (tank.CurrentFertilizer == null)
                {
                    tank.CurrentFertilizer = "empty";
                }
            }

            return View(tanks);
        }

        [HttpGet]
        [Authorize]
        public ActionResult AddFertilizer(int id)
        {
            var db = new WarehouseDbContext();

            var tank = db.Tanks
                .Where(t => t.Id == id)
                .Select(t => new TankViewModel
                {
                    Number = t.Number,
                    Name = t.Name,
                    CurrentLoad = t.CurrentLoad,
                    MaxCapacity = t.MaxCapacity,
                    CapacityLeft = t.MaxCapacity - t.CurrentLoad,
                    Fertilizer = new Fertilizer(),
                    CurrentFertilizer = t.CurrentFertilizer
                })
                .FirstOrDefault();

            if (tank.CurrentFertilizer == null)
            {
                tank.CurrentFertilizer = "empty";
            }

            if (tank == null)
            {
                return HttpNotFound();
            }

            return View(tank);
        }

        [HttpPost]
        [Authorize]
        public ActionResult AddFertilizer(TankViewModel model)
        {
            if (ModelState.IsValid)
            {
                var fertilizer = new Fertilizer
                {
                    Name = model.Fertilizer.Name,
                    Amount = model.Fertilizer.Amount
                };

                var operatorId = User.Identity.GetUserId();

                var db = new WarehouseDbContext();

                var operatorData = db.Users.Where(o => o.Id == operatorId).FirstOrDefault();

                var tank = db.Tanks.Find(model.Id);

                try
                {
                    tank.AddFertilizer(fertilizer, operatorData.FullName);
                    db.SaveChanges();

                    return RedirectToAction("All", "Tanks");
                }

                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"{ex.Message}");

                    var tankOther = db.Tanks
                                    .Where(t => t.Id == model.Id)
                                    .Select(t => new TankViewModel
                                    {
                                        Number = t.Number,
                                        Name = t.Name,
                                        CurrentLoad = t.CurrentLoad,
                                        MaxCapacity = t.MaxCapacity,
                                        CapacityLeft = t.MaxCapacity - t.CurrentLoad,
                                        Fertilizer = new Fertilizer()
                                    })
                                    .FirstOrDefault();

                    return View(tankOther);
                }
            }

            return View(model);

        }
    }
}