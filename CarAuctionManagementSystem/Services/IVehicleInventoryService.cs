using System.Collections.Generic;
using CarAuctionManagementSystem.Models;

namespace CarAuctionManagementSystem.Services
{
    public interface IVehicleInventoryService
    {
        /// <summary>
        /// Adds a vehicle to the inventory
        /// </summary>
        /// <param name="vehicle">The vehicle to add</param>
        /// <exception cref="CarAuctionManagementSystem.Exceptions.DuplicateVehicleException">Thrown when a vehicle with the same ID already exists</exception>
        void AddVehicle(Vehicle vehicle);

        /// <summary>
        /// Gets a vehicle from the inventory by ID
        /// </summary>
        /// <param name="vehicleId">ID of the vehicle to retrieve</param>
        /// <returns>The vehicle if found</returns>
        /// <exception cref="CarAuctionManagementSystem.Exceptions.VehicleNotFoundException">Thrown when no vehicle with the specified ID exists</exception>
        Vehicle GetVehicle(string vehicleId);

        /// <summary>
        /// Checks if a vehicle with the specified ID exists in the inventory
        /// </summary>
        /// <param name="vehicleId">ID to check</param>
        /// <returns>True if the vehicle exists, false otherwise</returns>
        bool VehicleExists(string vehicleId);

        /// <summary>
        /// Searches for vehicles matching the specified criteria
        /// </summary>
        /// <param name="type">Optional vehicle type filter</param>
        /// <param name="manufacturer">Optional manufacturer filter</param>
        /// <param name="model">Optional model filter</param>
        /// <param name="year">Optional year filter</param>
        /// <returns>A list of vehicles matching the criteria</returns>
        IEnumerable<Vehicle> SearchVehicles(VehicleType? type = null, string? manufacturer = null, string? model = null, int? year = null);
    }
} 