using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Taxi_04.DAL;
using Taxi_04.Models;

namespace Taxi_04.Controllers
{
    [Authorize(Roles = "Driver,Admin")]
    public class VehiclesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Vehicles
        public ActionResult Index()
        {
            var vehicles = db.Vehicles.Include(v => v.Driver);
            return View(vehicles.ToList());
        }

        // GET: Vehicles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Vehicle vehicle = db.Vehicles.Find(id); 
            Vehicle vehicle = db.Vehicles.Include(v => v.Driver).SingleOrDefault(x => x.ID == id); //Ovo je modificirano
            if (vehicle == null)
            {
                return HttpNotFound();
            }
            return View(vehicle);
        }

        // GET: Vehicles/Create
        public ActionResult Create(int? driverId)
        {
            if (driverId != null)
            {
                var driver = db.Drivers.Find(driverId);
                ViewBag.DriverId = new SelectList(db.Drivers, "ID", "Fullname", driver.Name);
            }
            else
            {
                ViewBag.DriverId = new SelectList(db.Drivers, "ID", "Fullname");
            }

            return View();
        }

        // POST: Vehicles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Brand,Model,MaxPerson,PricePerKm,DriverId")] Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {
                db.Vehicles.Add(vehicle);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DriverId = new SelectList(db.Drivers, "ID", "Fullname", vehicle.DriverId);
            return View(vehicle);
        }

        // GET: Vehicles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vehicle vehicle = db.Vehicles.Find(id);
            if (vehicle == null)
            {
                return HttpNotFound();
            }
            ViewBag.DriverId = new SelectList(db.Drivers, "ID", "Fullname", vehicle.DriverId);
            return View(vehicle);
        }

        // POST: Vehicles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Brand,Model,MaxPerson,PricePerKm,DriverId")] Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vehicle).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DriverId = new SelectList(db.Drivers, "ID", "Name", vehicle.DriverId);
            return View(vehicle);
        }

        // GET: Vehicles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vehicle vehicle = db.Vehicles.Include(v => v.Driver).SingleOrDefault(x => x.ID == id); //Ovo je modificirano
            if (vehicle == null)
            {
                return HttpNotFound();
            }
            return View(vehicle);
        }

        // POST: Vehicles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Vehicle vehicle = db.Vehicles.Find(id);
            db.Vehicles.Remove(vehicle);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult UpdateVehiclesForDriver(int driverId)
        {
            //Vehicle newVehicles = db.Vehicles.Include(v => v.Driver).Where(x => x.driverId == driverId);
            var newVehicles = db.Vehicles.Where(x => x.DriverId == driverId).ToList();
            return Json(newVehicles, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
