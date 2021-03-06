﻿using Microsoft.AspNet.Identity;
using System;
using System.Linq;
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
                if (model.CurrentCommodity == null)
                {
                    silo.CurrentCommodity = null;
                }

                else
                {
                    silo.CurrentCommodity = model.CurrentCommodity;
                }

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
                    Commodity = new Commodity(),
                    CurrentCommodity = s.CurrentCommodity
                })
                .FirstOrDefault();

            if (silo.CurrentCommodity == null)
            {
                silo.CurrentCommodity = "empty";
            }

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

                var operatorId = User.Identity.GetUserId();

                var db = new WarehouseDbContext();

                var operatorData = db.Users.Where(o => o.Id == operatorId).FirstOrDefault();

                var silo = db.Silos.Find(model.Id);

                try
                {
                    silo.AddCommodity(commodity, operatorData.FullName);
                    db.SaveChanges();

                    return RedirectToAction("All", "Silos");
                }

                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"{ex.Message}");

                    var siloOther = db.Silos
                                    .Where(s => s.Id == model.Id)
                                    .Select(s => new SiloViewModel
                                    {
                                        Number = s.SiloNumber,
                                        Name = s.Name,
                                        CurrentLoad = s.CurrentLoad,
                                        MaxCapacity = s.MaxCapacity,
                                        CapacityLeft = s.MaxCapacity - s.CurrentLoad,
                                        Commodity = new Commodity(),
                                        CurrentCommodity = s.CurrentCommodity
                                    })
                                    .FirstOrDefault();

                    return View(siloOther);
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
                    Commodity = new Commodity(),
                    CurrentCommodity = s.CurrentCommodity
                })
                .FirstOrDefault();

            if (silo.CurrentCommodity == null)
            {
                silo.CurrentCommodity = "empty";
            }

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

                var operatorId = User.Identity.GetUserId();

                var db = new WarehouseDbContext();

                var operatorData = db.Users.Where(o => o.Id == operatorId).FirstOrDefault();

                var silo = db.Silos.Find(model.Id);

                try
                {
                    silo.ExportCommodity(commodity, operatorData.FullName);
                    db.SaveChanges();

                    return RedirectToAction("All", "Silos");
                }

                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"{ex.Message}");

                    var siloOther = db.Silos
                                    .Where(s => s.Id == model.Id)
                                    .Select(s => new SiloViewModel
                                    {
                                        Number = s.SiloNumber,
                                        Name = s.Name,
                                        CurrentLoad = s.CurrentLoad,
                                        MaxCapacity = s.MaxCapacity,
                                        CapacityLeft = s.MaxCapacity - s.CurrentLoad,
                                        Commodity = new Commodity(),
                                        CurrentCommodity = s.CurrentCommodity
                                    })
                                    .FirstOrDefault();

                    return View(siloOther);
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

        public ActionResult AllOperations(int id, int page = 1)
        {
            var pageSize = 12;

            double size = 12;

            var db = new WarehouseDbContext();

            var siloQuery = db.Operations.AsQueryable();

            var totalActions = siloQuery
                .Where(a => a.SiloId == id)
                .Select(a => new AllOperationsModel
                {
                    Id = a.Id,
                    ActionDate = a.ActionDate,
                    AmountBeforeAction = a.AmountBeforeAction,
                    OperationName = a.OperationName,
                    ActionAmount = a.ActionAmount,
                    AmountAfterAction = a.AmountAfterAction,
                    SiloId = a.SiloId,
                    Operator = a.OperationName,
                    CommodityName = a.CommodityName
                })
                .ToList();

            var actions = siloQuery
                .Where(a => a.SiloId == id)
                .OrderByDescending(a => a.ActionDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(a => new AllOperationsModel
                {
                    Id = a.Id,
                    ActionDate = a.ActionDate,
                    AmountBeforeAction = a.AmountBeforeAction,
                    OperationName = a.OperationName,
                    ActionAmount = a.ActionAmount,
                    AmountAfterAction = a.AmountAfterAction,
                    SiloId = a.SiloId,
                    Operator = a.OperatorName,
                    CommodityName = a.CommodityName
                })
                .ToList();

            var totalPages = Math.Ceiling(totalActions.Count / size);

            ViewBag.CurrentPage = page;
            ViewBag.CurrentSilo = id;
            ViewBag.TotalPages = totalPages;

            return View(actions);
        }
    }
}