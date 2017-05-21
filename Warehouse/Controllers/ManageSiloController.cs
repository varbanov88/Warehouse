using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
            var silo = db.Silos.Find(id);

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
    }
}