using System;
using System.Collections.Generic;
using System.Linq;
using CarAuctionManagementSystem.Exceptions;
using CarAuctionManagementSystem.Models;

namespace CarAuctionManagementSystem.Services
{
    public class VehicleInventoryService : IVehicleInventoryService
    {
        private readonly Dictionary<string, Vehicle> _vehicles = new Dictionary<string, Vehicle>();

        public void AddVehicle(Vehicle vehicle)
        {
            if (vehicle == null)
                throw new ArgumentNullException(nameof(vehicle));

            if (_vehicles.ContainsKey(vehicle.Id))
                throw new DuplicateVehicleException(vehicle.Id);

            _vehicles.Add(vehicle.Id, vehicle);
        }

        public Vehicle GetVehicle(string vehicleId)
        {
            if (string.IsNullOrWhiteSpace(vehicleId))
                throw new ArgumentException("Vehicle ID cannot be empty", nameof(vehicleId));

            if (!_vehicles.TryGetValue(vehicleId, out Vehicle? vehicle) || vehicle is null)
                throw new VehicleNotFoundException(vehicleId);

            return vehicle;
        }

        public bool VehicleExists(string vehicleId)
        {
            if (string.IsNullOrWhiteSpace(vehicleId))
                throw new ArgumentException("Vehicle ID cannot be empty", nameof(vehicleId));

            return _vehicles.ContainsKey(vehicleId);
        }

        public IEnumerable<Vehicle> SearchVehicles(VehicleType? type = null, string? manufacturer = null, string? model = null, int? year = null)
        {
            IEnumerable<Vehicle> query = _vehicles.Values;

            if (type.HasValue)
                query = query.Where(v => v.Type == type.Value);

            if (!string.IsNullOrWhiteSpace(manufacturer))
                query = query.Where(v => v.Manufacturer.Contains(manufacturer, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(model))
                query = query.Where(v => v.Model.Contains(model, StringComparison.OrdinalIgnoreCase));

            if (year.HasValue)
                query = query.Where(v => v.Year == year.Value);

            return query.ToList();
        }
    }
}