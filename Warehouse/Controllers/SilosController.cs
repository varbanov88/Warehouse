using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Warehouse.Data;
using Warehouse.Models.Silos;
using YaraTask.Data;
using YaraTask.Models.Silos;

namespace YaraTask.Controllers
{
    public class SilosController : Controller
    {
        [Authorize]
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Create(CreateSiloModel model)
        {
            if (ModelState.IsValid)
            {
                var db = new WarehouseDbContext();
                var creatorId = this.User.Identity.GetUserId();

                var silo = new Silo(model.Name, model.MaxCapacity, model.SiloNumber);
                silo.SiloCreatorId = creatorId;

                db.Silos.Add(silo);
                db.SaveChanges();

                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        [Authorize]
        [HttpGet]
        public ActionResult AddCommodity(int id)
        {
            var db = new WarehouseDbContext();

            var silo = db.Silos
                .Where(s => s.Id == id)
                .Select(s => new SiloViewModel
                {
                    Number = s.SiloNumber,
                    Name = s.Name,
                    CurrentLoad = s.CurrentLoad,
                    MaxCapacity = s.MaxCapacity,
                    CapacityLeft = s.MaxCapacity - s.CurrentLoad,
                    Commodity = new Commodity() 
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
        public ActionResult AddCommodity(SiloViewModel model)
        {
            if (ModelState.IsValid)
            {
                var commodity = new Commodity
                {
                    Name = model.Commodity.Name,
                    Amount = model.Commodity.Amount
                };

                var db = new WarehouseDbContext();

                var silo = db.Silos.Find(model.Id);

                try
                {
                    silo.AddCommodity(commodity);
                    db.SaveChanges();

                    return RedirectToAction("All", "Silos");
                }

                catch (Exception ex)
                {
                    //to do: fix exception model bug
                    ModelState.AddModelError("", $"{ex.Message}");
                    return View(model);
                }
            }

            return View(model);
        }

        [HttpGet]
        [Authorize]
        public ActionResult ExportCommodity(int id)
        {
            var db = new WarehouseDbContext();

            var silo = db.Silos
                .Where(s => s.Id == id)
                .Select(s => new SiloViewModel
                {
                    Number = s.SiloNumber,
                    Name = s.Name,
                    CurrentLoad = s.CurrentLoad,
                    MaxCapacity = s.MaxCapacity,
                    CapacityLeft = s.MaxCapacity - s.CurrentLoad,
                    Commodity = new Commodity()
                })
                .FirstOrDefault();

            if (silo == null)
            {
                return HttpNotFound();
            }

            return View(silo);
        }

        [HttpPost]
        [Authorize]
        public ActionResult ExportCommodity(SiloViewModel model)
        {
            if (ModelState.IsValid)
            {
                var commodity = new Commodity
                {
                    Name = model.Commodity.Name,
                    Amount = model.Commodity.Amount
                };

                var db = new WarehouseDbContext();

                var silo = db.Silos.Find(model.Id);

                try
                {
                    silo.ExportCommodity(commodity);
                    db.SaveChanges();

                    return RedirectToAction("All", "Silos");
                }

                catch (Exception ex)
                {
                    return Json(new { status = "error", message = ex.Message });
                }
            }

            return View(model);
        }


        public ActionResult All()
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
                    CapacityLeft = s.MaxCapacity - s.CurrentLoad
                })
                .ToList();

            return View(silos);
        }

        public ActionResult AllOperations(int id)
        {
            var db = new WarehouseDbContext();

            var pasteQuery = db.Operations.AsQueryable();

            var actions = pasteQuery
                .OrderBy(a => a.Id)
                .Select(a => new AllOperationsModel
                {
                    Id = a.Id,
                    ActionDate = a.ActionDate,
                    AmountBeforeAction = a.AmountBeforeAction,
                    OperationName = a.OperationName,
                    ActionAmount = a.ActionAmount,
                    AmountAfterAction = a.AmountAfterAction
                })
                .ToList();

            return View(actions);
        }
    }
}