using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Warehouse.Data;
using Warehouse.Models.Tankers;

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

                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }
    }
}