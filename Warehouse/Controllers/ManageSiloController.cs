using System;
using System.Linq;
using System.Web.Mvc;
using Warehouse.Data;
using Warehouse.Models.Silos;

namespace Warehouse.Controllers
{
    public class ManageSiloController : Controller
    {
        public ActionResult AllSilos()
        {
            var db = new WarehouseDbContext();

            var silos = db.Silos
                .OrderBy(s => s.SiloNumber)
                .Select(s => new AllSilosViewModel
                {
                    Id = s.Id,
                    Number = s.SiloNumber,
                    Name = s.Name,
                    MaxCapacity = s.MaxCapacity,
                    CapacityLeft = s.MaxCapacity - s.CurrentLoad,
                    CurrentCommodity = s.CurrentCommodity
                })
                .ToList();

            foreach (var silo in silos)
            {
                if (silo.CurrentCommodity == null)
                {
                    silo.CurrentCommodity = "empty";
                }
            }

            return View(silos);
        }

        [Authorize]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var db = new WarehouseDbContext();

            var siloQuery = db.Silos.AsQueryable();

            var silo = siloQuery
                .Where(a => a.Id == id)
                .Select(a => new EditSiloModel
                {
                    Id = a.Id,
                    Name = a.Name,
                    MaxCapacity = a.MaxCapacity,
                    Number = a.SiloNumber
                })
                .FirstOrDefault();

            if (silo == null)
            {
                return HttpNotFound();
            }

            return View(silo);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Edit(EditSiloModel model)
        {
            if (ModelState.IsValid)
            {
                using (var db = new WarehouseDbContext())
                {
                    var silo = db.Silos.Find(model.Id);

                    if (silo == null)
                    {
                        return HttpNotFound();
                    }

                    silo.Name = model.Name;
                    silo.SiloNumber = model.Number;
                    silo.MaxCapacity = model.MaxCapacity;

                    db.SaveChanges();
                }

                return RedirectToAction("AllSilos");
            }

            return View(model);
        }

        [Authorize]
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var db = new WarehouseDbContext();

            var siloQuery = db.Silos.AsQueryable();

            var silo = siloQuery
                .Where(a => a.Id == id)
                .Select(a => new DeleteSiloModel
                {
                    Id = a.Id,
                    Name = a.Name,
                    MaxCapacity = a.MaxCapacity,
                    Number = a.SiloNumber,
                    CurrentCommodity = a.CurrentCommodity,
                    CurrentLoad = a.CurrentLoad,
                    CapacityLeft = a.MaxCapacity - a.CurrentLoad
                })
                .FirstOrDefault();

            var sillo = db.Silos.Find(id);

            if (silo == null)
            {
                return HttpNotFound();
            }

            try
            {
                sillo.CanDeleteSilo(silo);
                ViewBag.Id = silo.Id;
                return View(silo);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"{ex.Message}");
                return RedirectToAction("AllSilos");
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult Delete(DeleteSiloModel model, int id)
        {
            var db = new WarehouseDbContext();

            var siloQuery = db.Silos.AsQueryable();

            var silo = siloQuery
                .Where(a => a.Id == id)
                .Select(a => new DeleteSiloModel
                {
                    Id = id,
                    Name = a.Name,
                    MaxCapacity = a.MaxCapacity,
                    Number = a.SiloNumber,
                    CurrentCommodity = a.CurrentCommodity
                })
                .FirstOrDefault();

            if (silo == null)
            {
                return HttpNotFound();
            }

            var sillo = db.Silos.Find(id);

            db.Silos.Remove(sillo);
            var operations = db.Operations.Where(c => c.SiloId == id).ToList();

            foreach (var op in operations)
            {
                db.Operations.Remove(op);
            }

            db.SaveChanges();
            return RedirectToAction("AllSilos");
        }
    }
}