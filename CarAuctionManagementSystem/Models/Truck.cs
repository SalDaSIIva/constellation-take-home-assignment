using System;

namespace CarAuctionManagementSystem.Models
{
    public class Truck : Vehicle
    {
        public decimal LoadCapacity { get; } // tons

        public Truck(string id, string manufacturer, string model, int year, decimal startingBid, decimal loadCapacity)
            : base(id, manufacturer, model, year, startingBid, VehicleType.Truck)
        {
            if (loadCapacity <= 0)
                throw new ArgumentOutOfRangeException(nameof(loadCapacity), "Load capacity must be greater than zero");

            LoadCapacity = loadCapacity;
        }
    }
} 