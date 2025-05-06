using System;
using System.Collections.Generic;
using CarAuctionManagementSystem.Models;

namespace CarAuctionManagementSystem.Services
{
    /// <summary>
    /// Facade class that integrates all system components and provides simplified access
    /// </summary>
    public class AuctionManagementSystem
    {
        private readonly IVehicleInventoryService _inventoryService;
        private readonly IAuctionService _auctionService;

        public AuctionManagementSystem()
        {
            _inventoryService = new VehicleInventoryService();
            _auctionService = new AuctionService(_inventoryService);
        }

        #region Vehicle Inventory Operations

        public void AddVehicle(Vehicle vehicle) => _inventoryService.AddVehicle(vehicle);

        public Vehicle GetVehicleById(string vehicleId) => _inventoryService.GetVehicle(vehicleId);

        public bool VehicleExists(string vehicleId) => _inventoryService.VehicleExists(vehicleId);

        public IEnumerable<Vehicle> SearchVehicles(VehicleType? type = null, string? manufacturer = null, string? model = null, int? year = null) => 
            _inventoryService.SearchVehicles(type, manufacturer, model, year);

        #endregion

        #region Auction Operations

        public Auction StartAuction(string vehicleId) => _auctionService.StartAuction(vehicleId);

        public void PlaceBid(string vehicleId, string bidder, decimal amount) => 
            _auctionService.PlaceBid(vehicleId, bidder, amount);

        public Auction CloseAuction(string vehicleId) => _auctionService.CloseAuction(vehicleId);

        public Auction? GetActiveAuction(string vehicleId) => _auctionService.GetActiveAuction(vehicleId);

        public IEnumerable<Auction> GetAllActiveAuctions() => _auctionService.GetAllActiveAuctions();

        public IEnumerable<Auction> GetAuctionHistory(string vehicleId) => _auctionService.GetAuctionHistory(vehicleId);

        #endregion

        #region Convenience Factory Methods

        public Sedan CreateSedan(string id, string manufacturer, string model, int year, decimal startingBid, int numberOfDoors) =>
            new Sedan(id, manufacturer, model, year, startingBid, numberOfDoors);

        public Hatchback CreateHatchback(string id, string manufacturer, string model, int year, decimal startingBid, int numberOfDoors) =>
            new Hatchback(id, manufacturer, model, year, startingBid, numberOfDoors);

        public SUV CreateSUV(string id, string manufacturer, string model, int year, decimal startingBid, int numberOfSeats) =>
            new SUV(id, manufacturer, model, year, startingBid, numberOfSeats);

        public Truck CreateTruck(string id, string manufacturer, string model, int year, decimal startingBid, decimal loadCapacity) =>
            new Truck(id, manufacturer, model, year, startingBid, loadCapacity);

        #endregion
    }
} 