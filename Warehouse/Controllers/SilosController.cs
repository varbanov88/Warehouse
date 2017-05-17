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
        public ActionResult AddCommodity()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult AddCommodity(AddCommodityModel model)
        {
            if (ModelState.IsValid)
            {
                var commodity = new Commodity
                {
                    Name = model.Name,
                    Amount = model.Amount
                };

                var db = new WarehouseDbContext();
                
            }

            return View(model);
        }
    }
}