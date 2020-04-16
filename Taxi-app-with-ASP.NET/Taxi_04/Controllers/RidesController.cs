using GeoCoordinatePortable;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Taxi_04.DAL;
using Taxi_04.Models;

namespace Taxi_04.Controllers
{
    [Authorize(Roles = "Passenger,Driver,Admin")]
    public class RidesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Rides
        public ActionResult Index()
        {
            //NOVO
            var rides = db.Rides.Include(r => r.Driver).Include(r => r.EndLocation).Include(r => r.Passenger).Include(r => r.StartLocation).Include(r => r.Vehicle);
            List<double> prices = new List<double>(); ;

            foreach (var ride in rides)
            {
                IList<string> startLocationValues = ride.StartLocation.Location.Split(',');
                float startLat = float.Parse(startLocationValues[0], CultureInfo.InvariantCulture.NumberFormat);
                float startLon = float.Parse(startLocationValues[1], CultureInfo.InvariantCulture.NumberFormat);
                GeoCoordinate pin1 = new GeoCoordinate(startLat, startLon);

                IList<string> endLocationValues = ride.EndLocation.Location.Split(',');
                float endLat = float.Parse(endLocationValues[0], CultureInfo.InvariantCulture.NumberFormat);
                float endLon = float.Parse(endLocationValues[1], CultureInfo.InvariantCulture.NumberFormat);
                GeoCoordinate pin2 = new GeoCoordinate(endLat, endLon);

                double distanceBetweenInKm = pin1.GetDistanceTo(pin2) / 1000;
                ride.Price = (int) (distanceBetweenInKm *ride.Vehicle.PricePerKm);
            }
            //KRAJ NOVO

            return View(rides.ToList());
        }

        // GET: Rides/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ride ride = db.Rides.Find(id);


            //NOVO
            IList<string> startLocationValues = ride.StartLocation.Location.Split(',');
            float startLat = float.Parse(startLocationValues[0], CultureInfo.InvariantCulture.NumberFormat);
            float startLon = float.Parse(startLocationValues[1], CultureInfo.InvariantCulture.NumberFormat);
            GeoCoordinate pin1 = new GeoCoordinate(startLat, startLon);

            IList<string> endLocationValues = ride.EndLocation.Location.Split(',');
            float endLat = float.Parse(endLocationValues[0], CultureInfo.InvariantCulture.NumberFormat);
            float endLon = float.Parse(endLocationValues[1], CultureInfo.InvariantCulture.NumberFormat);
            GeoCoordinate pin2 = new GeoCoordinate(endLat, endLon);

            double distanceBetweenInKm = pin1.GetDistanceTo(pin2)/1000;
            ride.Price = (int)(distanceBetweenInKm * ride.Vehicle.PricePerKm);
            //KRAJ NOVO

            if (ride == null)
            {
                return HttpNotFound();
            }
            return View(ride);
        }

        // GET: Rides/Create
        public ActionResult Create(int? driverId)
        {
            ViewBag.PassengerId = new SelectList(db.Passengers, "ID", "Fullname");
            ViewBag.StartLocationId = new SelectList(db.Cities, "ID", "Name");
            ViewBag.EndLocationId = new SelectList(db.Cities, "ID", "Name");

			var drivers = db.Drivers;
			
            if (driverId != null)
            {
                var chosenDriver = drivers.Find(driverId);
                int chosenDriverId = chosenDriver.ID;
                ViewBag.DriverId = new SelectList(drivers, "ID", "Fullname", chosenDriver.Name);
                ViewBag.VehicleId = new SelectList(db.Vehicles.Where(b => b.DriverId.Equals(chosenDriverId)), "ID", "Fullname");
            }
            else
            {
                int firstId = drivers.FirstOrDefault().ID;
                ViewBag.DriverId = new SelectList(drivers, "ID", "Fullname"); //dodano
                ViewBag.VehicleId = new SelectList(db.Vehicles.Where(b => b.DriverId.Equals(firstId)), "ID", "Fullname");
            }

            return View();
        }

        // POST: Rides/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,StartLocationId,EndLocationId,VehicleId,DriverId,PassengerId,Time")] Ride ride)
        {
            if (ModelState.IsValid)
            {
                db.Rides.Add(ride);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
			
			var drivers = db.Drivers;
            int firstId = drivers.FirstOrDefault().ID;
            ViewBag.DriverId = new SelectList(drivers, "ID", "Fullname", ride.DriverId);
			ViewBag.StartLocationId = new SelectList(db.Cities, "ID", "Name", ride.StartLocationId);
			ViewBag.EndLocationId = new SelectList(db.Cities, "ID", "Name", ride.EndLocationId);
			ViewBag.PassengerId = new SelectList(db.Passengers, "ID", "Fullname", ride.PassengerId);
            ViewBag.VehicleId = new SelectList(db.Vehicles.Where(b => b.DriverId.Equals(firstId)), "ID", "Fullname", ride.VehicleId);
            return View(ride);
        }

        // GET: Rides/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ride ride = db.Rides.Find(id);
            if (ride == null)
            {
                return HttpNotFound();
            }
			ViewBag.DriverId = new SelectList(db.Drivers, "ID", "Fullname", ride.DriverId);
            ViewBag.PassengerId = new SelectList(db.Passengers, "ID", "Fullname", ride.PassengerId);
            ViewBag.StartLocationId = new SelectList(db.Cities, "ID", "Name", ride.StartLocationId);
            ViewBag.EndLocationId = new SelectList(db.Cities, "ID", "Name", ride.EndLocationId);
			ViewBag.VehicleId = new SelectList(db.Vehicles.Where(b => b.DriverId.Equals(ride.DriverId.Value)), "ID", "Fullname", ride.VehicleId);
            return View(ride);
        }

        // POST: Rides/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,StartLocationId,EndLocationId,VehicleId,DriverId,PassengerId,Time")] Ride ride)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ride).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
			ViewBag.DriverId = new SelectList(db.Drivers, "ID", "Fullname", ride.DriverId);
			ViewBag.StartLocationId = new SelectList(db.Cities, "ID", "Name", ride.StartLocationId);
            ViewBag.EndLocationId = new SelectList(db.Cities, "ID", "Name", ride.EndLocationId);
            ViewBag.PassengerId = new SelectList(db.Passengers, "ID", "Fullname", ride.PassengerId);
            ViewBag.VehicleId = new SelectList(db.Vehicles, "ID", "Fullname", ride.VehicleId);
            return View(ride);
        }

        // GET: Rides/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ride ride = db.Rides.Find(id);
            if (ride == null)
            {
                return HttpNotFound();
            }
            return View(ride);
        }

        // POST: Rides/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ride ride = db.Rides.Find(id);
            db.Rides.Remove(ride);
            db.SaveChanges();
            return RedirectToAction("Index");
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
