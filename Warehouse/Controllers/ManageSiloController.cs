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
                    CapacityLeft = a.MaxCapacity - a.CurrentLoad,
                    CurrentCommodity = a.CurrentCommodity,
                    CurrentLoad = a.CurrentLoad
                })
                .FirstOrDefault();

            if (silo == null)
            {
                return HttpNotFound();
            }

            return View(silo);
        }

        [Authorize]
        [ActionName("Delete")]
        [HttpPost]
        public ActionResult ConfirmDelete(int id , DeleteSiloModel model)
        {
            if (ModelState.IsValid)
            {
                var db = new WarehouseDbContext();

                var silo = db.Silos
                    .Where(a => a.Id == id)
                    .FirstOrDefault();

                var siloCheck = new DeleteSiloModel
                {
                    CurrentCommodity = silo.CurrentCommodity,
                    CurrentLoad = silo.CurrentLoad,
                    CapacityLeft = silo.MaxCapacity - silo.CurrentLoad,
                    Name = silo.Name
                };

                if (silo == null)
                {
                    return HttpNotFound();
                }

                try
                {
                    silo.CanDeleteSilo(siloCheck);
                    db.Silos.Remove(silo);

                    var operations = db.Operations.Where(o => o.SiloId == id).ToList();

                    foreach (var op in operations)
                    {
                        db.Operations.Remove(op);
                    }

                    db.SaveChanges();
                    return RedirectToAction("AllSilos");

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