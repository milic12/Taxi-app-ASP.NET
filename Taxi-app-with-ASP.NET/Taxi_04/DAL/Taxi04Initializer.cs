using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Taxi_04.Models;

namespace Taxi_04.DAL
{
    public class Taxi04Initilizer : DropCreateDatabaseAlways<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            var passengers = new List<Passenger>()
            {
                new Passenger() { Name = "Pero", Surname = "Peric" },
                new Passenger() { Name = "Ivo", Surname = "Ivic" },
                new Passenger() { Name = "Ante", Surname = "Antic" }
            };

            var vehicles = new List<Vehicle>()
            {
                new Vehicle() { Brand = "Mazda", Model = "Proton", MaxPerson = 5, PricePerKm = 1.0F },
                new Vehicle() { Brand = "Honda", Model = "Accord", MaxPerson = 5, PricePerKm = 0.5F },
                new Vehicle() { Brand = "Hyundai", Model = "Santa Fe", MaxPerson = 5, PricePerKm = 0.7F },
                new Vehicle() { Brand = "Porche", Model = "911 Carrera", MaxPerson = 3, PricePerKm = 8.0F }
            };

            var cities = new List<City>()
            {
                new City()
                {
                    Name = "Split",
                    Location = "43.514236, 16.458009"
                },
                new City()
                {
                    Name = "Zagreb",
                    Location = "45.812724, 15.976092"
                },
                new City()
                {
                    Name = "Rijeka",
                    Location = "45.329384, 15.440203"
                }
            };

            var drivers = new List<Driver>()
            {
                new Driver()
                {
                    Name ="Marko",
                    Surname = "Markovic",
                    OwnedVehicles = new List<Vehicle>() { vehicles[0], vehicles[1] }
                },
                new Driver()
                {
                    Name ="Jure",
                    Surname = "Juric",
                    OwnedVehicles = new List<Vehicle>() { vehicles[2], vehicles[3] }
                }
            };

            var rides = new List<Ride>()
            {
                new Ride()
                {
                    Passenger =  passengers[0],
                    Vehicle = vehicles[0],
                    StartLocation = cities[0],
                    EndLocation = cities[1],
                    Driver = drivers[0],
                    Time = DateTime.Now,
                },
                new Ride()
                {
                    Passenger =  passengers[2],
                    Vehicle = vehicles[2],
                    StartLocation = cities[2],
                    EndLocation = cities[1],
                    Driver = drivers[1],
                    Time = DateTime.Now.AddDays(10),
                }
            };



            context.Passengers.AddRange(passengers);
            context.Drivers.AddRange(drivers);
            context.Rides.AddRange(rides);

            context.SaveChanges();
        }
    }
}