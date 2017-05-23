using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Web.Mvc;
using Warehouse.Data;
using Warehouse.Models.Tankers;
using Warehouse.Models.Tanks;

namespace Warehouse.Controllers
{
    public class ManageTankController : Controller
    {
        [Authorize]
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Create(CreateTankModel model)
        {
            if (ModelState.IsValid)
            {
                var db = new WarehouseDbContext();
                var creatorId = this.User.Identity.GetUserId();

                var tank = new Tank(model.Name, model.TankNumber, model.MaxCapacity);
                tank.TankCreatorId = creatorId;
                if (model.CurrentFertilizer == null)
                {
                    tank.CurrentFertilizer = null;
                }

                else
                {
                    tank.CurrentFertilizer = model.CurrentFertilizer.ToLower();
                }

                db.Tanks.Add(tank);
                db.SaveChanges();

                return RedirectToAction("All", "Tanks");
            }

            return View(model);
        }

        public ActionResult AllTanks()
        {
            var db = new WarehouseDbContext();

            var tanks = db.Tanks
                .OrderBy(t => t.Number)
                .Select(t => new AllTanksViewModel
                {
                    Id = t.Id,
                    Number = t.Number,
                    Name = t.Name,
                    CurrentFertilizer = t.CurrentFertilizer,
                    MaxCapacity = t.MaxCapacity,
                    CapacityLeft = t.MaxCapacity - t.CurrentLoad
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

        [Authorize]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var db = new WarehouseDbContext();

            var tankQuery = db.Tanks.AsQueryable();

            var tank = tankQuery
                .Where(t => t.Id == id)
                .Select(t => new EditTankModel
                {
                    Id = t.Id,
                    Name = t.Name,
                    MaxCapacity = t.MaxCapacity,
                    Number = t.Number
                })
                .FirstOrDefault();

            if (tank == null)
            {
                return HttpNotFound();
            }

            return View(tank);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Edit(EditTankModel model)
        {
            if (ModelState.IsValid)
            {
                using (var db = new WarehouseDbContext())
                {
                    var tank = db.Tanks.Find(model.Id);

                    if (tank == null)
                    {
                        return HttpNotFound();
                    }

                    tank.Name = model.Name;
                    tank.Number = model.Number;
                    tank.MaxCapacity = model.MaxCapacity;

                    db.SaveChanges();
                }

                return RedirectToAction("AllTanks");
            }

            return View(model);
        }

        [Authorize]
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var db = new WarehouseDbContext();

            var tankQuery = db.Tanks.AsQueryable();

            var tank = tankQuery
                .Where(t => t.Id == id)
                .Select(t => new DeleteTankModel
                {
                    Id = t.Id,
                    Name = t.Name,
                    MaxCapacity = t.MaxCapacity,
                    Number = t.Number,
                    CapacityLeft = t.MaxCapacity - t.CurrentLoad,
                    CurrentFertilizer = t.CurrentFertilizer,
                    CurrentLoad = t.CurrentLoad
                })
                .FirstOrDefault();

            if (tank == null)
            {
                return HttpNotFound();
            }

            return View(tank);
        }

        [Authorize]
        [ActionName("Delete")]
        [HttpPost]
        public ActionResult ConfirmDelete(int id, DeleteTankModel model)
        {
            if (ModelState.IsValid)
            {
                var db = new WarehouseDbContext();

                var tank = db.Tanks
                    .Where(t => t.Id == id)
                    .FirstOrDefault();

                var tankCheck = new DeleteTankModel
                {
                    CurrentFertilizer = tank.CurrentFertilizer,
                    CurrentLoad = tank.CurrentLoad,
                    CapacityLeft = tank.MaxCapacity - tank.CurrentLoad,
                    Name = tank.Name
                };

                if (tank == null)
                {
                    return HttpNotFound();
                }

                try
                {
                    tank.CanDelete(tankCheck);
                    db.Tanks.Remove(tank);

                    var tankOperations = db.TankOperations.Where(o => o.TankId == id).ToList();

                    foreach (var to in tankOperations)
                    {
                        db.TankOperations.Remove(to);
                    }

                    db.SaveChanges();
                    return RedirectToAction("AllTanks");

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"{ex.Message}");

                    return View(model);
                }

            }

            return View(model);
        }

    }
}