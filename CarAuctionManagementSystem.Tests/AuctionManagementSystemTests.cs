using System;
using System.Linq;
using CarAuctionManagementSystem.Exceptions;
using CarAuctionManagementSystem.Models;
using CarAuctionManagementSystem.Services;
using Xunit;

namespace CarAuctionManagementSystem.Tests
{
    public class AuctionManagementSystemTests
    {
        private readonly AuctionManagementSystem _system;

        public AuctionManagementSystemTests()
        {
            _system = new AuctionManagementSystem();
        }

        [Fact]
        public void IntegrationTest_FullAuctionLifecycle()
        {
            // 1. Create different types of vehicles
            var sedan = _system.CreateSedan("SED001", "Toyota", "Camry", 2020, 18000m, 4);
            var suv = _system.CreateSUV("SUV001", "Ford", "Explorer", 2021, 35000m, 7);
            var hatchback = _system.CreateHatchback("HAT001", "Volkswagen", "Golf", 2019, 16000m, 5);
            var truck = _system.CreateTruck("TRK001", "Chevrolet", "Silverado", 2021, 32000m, 2.5m);

            // 2. Add vehicles to inventory
            _system.AddVehicle(sedan);
            _system.AddVehicle(suv);
            _system.AddVehicle(hatchback);
            _system.AddVehicle(truck);

            // 3. Verify search functionality
            var toyotaVehicles = _system.SearchVehicles(manufacturer: "Toyota").ToList();
            Assert.Single(toyotaVehicles);
            Assert.Equal("SED001", toyotaVehicles.First().Id);

            var vehicles2021 = _system.SearchVehicles(year: 2021).ToList();
            Assert.Equal(2, vehicles2021.Count);

            var suvs = _system.SearchVehicles(type: VehicleType.SUV).ToList();
            Assert.Single(suvs);
            Assert.Equal("SUV001", suvs.First().Id);

            // 4. Start an auction for a vehicle
            var auction = _system.StartAuction("SED001");
            Assert.NotNull(auction);
            Assert.True(auction.IsActive);
            Assert.Equal(18000m, auction.CurrentHighestBid);

            // 5. Place bids on the auction
            _system.PlaceBid("SED001", "Alex", 18500m);
            _system.PlaceBid("SED001", "Alvaro", 19000m);
            _system.PlaceBid("SED001", "Vlad", 19250m);

            // 6. Verify invalid bids are rejected
            Assert.Throws<InvalidBidException>(() => _system.PlaceBid("SED001", "Hannah", 19000m));

            // 7. Check auction status
            var activeAuction = _system.GetActiveAuction("SED001");
            Assert.NotNull(activeAuction);
            Assert.Equal("Vlad", activeAuction.CurrentHighestBidder);
            Assert.Equal(19250m, activeAuction.CurrentHighestBid);
            Assert.Equal(3, activeAuction.BidHistory.Count);

            // 8. Close the auction
            var closedAuction = _system.CloseAuction("SED001");
            Assert.False(closedAuction.IsActive);
            Assert.NotNull(closedAuction.EndTime);
            Assert.Equal("Vlad", closedAuction.CurrentHighestBidder);

            // 9. Start multiple auctions
            _system.StartAuction("SUV001");
            _system.StartAuction("HAT001");

            // 10. Verify active auctions
            var activeAuctions = _system.GetAllActiveAuctions().ToList();
            Assert.Equal(2, activeAuctions.Count);

            // 11. Test error handling
            Assert.Throws<VehicleNotFoundException>(() => _system.StartAuction("CYBERTRUCK"));
            Assert.Throws<AuctionNotFoundException>(() => _system.PlaceBid("TRK001", "Alex", 35000m));
            Assert.Throws<DuplicateVehicleException>(() => _system.AddVehicle(_system.CreateSedan("SED001", "Honda", "Accord", 2022, 21000m, 4)));
        }

        [Fact]
        public void FactoryMethods_CreateCorrectVehicleTypes()
        {
            // Arrange & Act
            var sedan = _system.CreateSedan("SED001", "Toyota", "Camry", 2020, 18000m, 4);
            var suv = _system.CreateSUV("SUV001", "Ford", "Explorer", 2021, 35000m, 7);
            var hatchback = _system.CreateHatchback("HAT001", "Volkswagen", "Golf", 2019, 16000m, 5);
            var truck = _system.CreateTruck("TRK001", "Chevrolet", "Silverado", 2021, 32000m, 2.5m);

            // Assert
            Assert.IsType<Sedan>(sedan);
            Assert.Equal(VehicleType.Sedan, sedan.Type);
            Assert.Equal(4, sedan.NumberOfDoors);

            Assert.IsType<SUV>(suv);
            Assert.Equal(VehicleType.SUV, suv.Type);
            Assert.Equal(7, suv.NumberOfSeats);

            Assert.IsType<Hatchback>(hatchback);
            Assert.Equal(VehicleType.Hatchback, hatchback.Type);
            Assert.Equal(5, hatchback.NumberOfDoors);

            Assert.IsType<Truck>(truck);
            Assert.Equal(VehicleType.Truck, truck.Type);
            Assert.Equal(2.5m, truck.LoadCapacity);
        }
    }
} 