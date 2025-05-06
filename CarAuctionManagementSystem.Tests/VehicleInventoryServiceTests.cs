using System;
using System.Linq;
using CarAuctionManagementSystem.Exceptions;
using CarAuctionManagementSystem.Models;
using CarAuctionManagementSystem.Services;
using Xunit;

namespace CarAuctionManagementSystem.Tests
{
    public class VehicleInventoryServiceTests
    {
        private readonly VehicleInventoryService _inventoryService;
        private readonly Sedan _testSedan;
        private readonly SUV _testSuv;
        private readonly Hatchback _testHatchback;
        private readonly Truck _testTruck;

        public VehicleInventoryServiceTests()
        {
            _inventoryService = new VehicleInventoryService();
            
            _testSedan = new Sedan("SED123", "Toyota", "Camry", 2020, 18000m, 4);
            _testSuv = new SUV("SUV123", "Ford", "Explorer", 2021, 35000m, 7);
            _testHatchback = new Hatchback("HAT123", "Volkswagen", "Golf", 2019, 16000m, 5);
            _testTruck = new Truck("TRK123", "Chevrolet", "Silverado", 2020, 32000m, 2.5m);
        }

        [Fact]
        public void AddVehicle_Success()
        {
            // Arrange - done in constructor
            
            // Act
            _inventoryService.AddVehicle(_testSedan);
            
            // Assert
            Assert.True(_inventoryService.VehicleExists(_testSedan.Id));
        }

        [Fact]
        public void AddVehicle_NullVehicle_ThrowsArgumentNullException()
        {
            // Arrange, Act & Assert
            Assert.Throws<ArgumentNullException>(() => _inventoryService.AddVehicle(null!));
        }

        [Fact]
        public void AddVehicle_DuplicateId_ThrowsDuplicateVehicleException()
        {
            // Arrange
            _inventoryService.AddVehicle(_testSedan);
            var duplicateSedan = new Sedan(_testSedan.Id, "Honda", "Accord", 2021, 20000m, 4);
            
            // Act & Assert
            Assert.Throws<DuplicateVehicleException>(() => _inventoryService.AddVehicle(duplicateSedan));
        }

        [Fact]
        public void GetVehicle_ExistingVehicle_ReturnsVehicle()
        {
            // Arrange
            _inventoryService.AddVehicle(_testSedan);
            
            // Act
            var vehicle = _inventoryService.GetVehicle(_testSedan.Id);
            
            // Assert
            Assert.NotNull(vehicle);
            Assert.Equal(_testSedan.Id, vehicle.Id);
            Assert.Equal(_testSedan.Manufacturer, vehicle.Manufacturer);
            Assert.Equal(_testSedan.Model, vehicle.Model);
        }

        [Fact]
        public void GetVehicle_NonExistingVehicle_ThrowsVehicleNotFoundException()
        {
            // Arrange, Act & Assert
            Assert.Throws<VehicleNotFoundException>(() => _inventoryService.GetVehicle("nonexistent"));
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public void GetVehicle_InvalidId_ThrowsArgumentException(string invalidId)
        {
            // Arrange, Act & Assert
            Assert.Throws<ArgumentException>(() => _inventoryService.GetVehicle(invalidId));
        }

        [Fact]
        public void GetVehicle_InvalidIdNull_ThrowsArgumentException()
        {
            // Arrange, Act & Assert
            Assert.Throws<ArgumentException>(() => _inventoryService.GetVehicle(null!));
        }

        [Fact]
        public void VehicleExists_ExistingVehicle_ReturnsTrue()
        {
            // Arrange
            _inventoryService.AddVehicle(_testSedan);
            
            // Act
            var exists = _inventoryService.VehicleExists(_testSedan.Id);
            
            // Assert
            Assert.True(exists);
        }

        [Fact]
        public void VehicleExists_NonExistingVehicle_ReturnsFalse()
        {
            // Arrange, Act
            var exists = _inventoryService.VehicleExists("nonexistent");
            
            // Assert
            Assert.False(exists);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public void VehicleExists_InvalidId_ThrowsArgumentException(string invalidId)
        {
            // Arrange, Act & Assert
            Assert.Throws<ArgumentException>(() => _inventoryService.VehicleExists(invalidId));
        }

        [Fact]
        public void VehicleExists_InvalidIdNull_ThrowsArgumentException()
        {
            // Arrange, Act & Assert
            Assert.Throws<ArgumentException>(() => _inventoryService.VehicleExists(null!));
        }

        [Fact]
        public void SearchVehicles_ByManufacturer_ReturnsMatchingVehicles()
        {
            // Arrange
            _inventoryService.AddVehicle(_testSedan);
            _inventoryService.AddVehicle(_testSuv);
            _inventoryService.AddVehicle(_testTruck);
            
            // Act
            var vehicles = _inventoryService.SearchVehicles(manufacturer: "Toyota").ToList();
            
            // Assert
            Assert.Single(vehicles);
            Assert.Equal(_testSedan.Id, vehicles.First().Id);
        }

        [Fact]
        public void SearchVehicles_ByType_ReturnsMatchingVehicles()
        {
            // Arrange
            _inventoryService.AddVehicle(_testSedan);
            _inventoryService.AddVehicle(_testSuv);
            _inventoryService.AddVehicle(_testTruck);
            
            // Act
            var vehicles = _inventoryService.SearchVehicles(type: VehicleType.SUV).ToList();
            
            // Assert
            Assert.Single(vehicles);
            Assert.Equal(_testSuv.Id, vehicles.First().Id);
        }

        [Fact]
        public void SearchVehicles_ByYear_ReturnsMatchingVehicles()
        {
            // Arrange
            _inventoryService.AddVehicle(_testSedan);
            _inventoryService.AddVehicle(_testSuv);
            _inventoryService.AddVehicle(_testTruck);
            
            // Act
            var vehicles = _inventoryService.SearchVehicles(year: 2020).ToList();
            
            // Assert
            Assert.Equal(2, vehicles.Count);
            Assert.Contains(vehicles, v => v.Id == _testSedan.Id);
            Assert.Contains(vehicles, v => v.Id == _testTruck.Id);
        }

        [Fact]
        public void SearchVehicles_ByModel_ReturnsMatchingVehicles()
        {
            // Arrange
            _inventoryService.AddVehicle(_testSedan);
            _inventoryService.AddVehicle(_testSuv);
            _inventoryService.AddVehicle(_testTruck);
            
            // Act
            var vehicles = _inventoryService.SearchVehicles(model: "Ex").ToList(); // Should match "Explorer"
            
            // Assert
            Assert.Single(vehicles);
            Assert.Equal(_testSuv.Id, vehicles.First().Id);
        }

        [Fact]
        public void SearchVehicles_CaseInsensitive_ReturnsMatchingVehicles()
        {
            // Arrange
            _inventoryService.AddVehicle(_testSedan);
            
            // Act
            var vehicles = _inventoryService.SearchVehicles(manufacturer: "toyota").ToList();
            
            // Assert
            Assert.Single(vehicles);
            Assert.Equal(_testSedan.Id, vehicles.First().Id);
        }

        [Fact]
        public void SearchVehicles_MultipleFilters_ReturnsMatchingVehicles()
        {
            // Arrange
            _inventoryService.AddVehicle(_testSedan);
            _inventoryService.AddVehicle(_testSuv);
            _inventoryService.AddVehicle(_testTruck);
            _inventoryService.AddVehicle(new Sedan("SED456", "Toyota", "Corolla", 2021, 17000m, 4));
            
            // Act
            var vehicles = _inventoryService.SearchVehicles(
                manufacturer: "Toyota", 
                year: 2020
            ).ToList();
            
            // Assert
            Assert.Single(vehicles);
            Assert.Equal(_testSedan.Id, vehicles.First().Id);
        }

        [Fact]
        public void SearchVehicles_NoFilters_ReturnsAllVehicles()
        {
            // Arrange
            _inventoryService.AddVehicle(_testSedan);
            _inventoryService.AddVehicle(_testSuv);
            _inventoryService.AddVehicle(_testTruck);
            
            // Act
            var vehicles = _inventoryService.SearchVehicles().ToList();
            
            // Assert
            Assert.Equal(3, vehicles.Count);
        }

        [Fact]
        public void SearchVehicles_NoMatches_ReturnsEmptyList()
        {
            // Arrange
            _inventoryService.AddVehicle(_testSedan);
            _inventoryService.AddVehicle(_testSuv);
            
            // Act
            var vehicles = _inventoryService.SearchVehicles(manufacturer: "NonExistent").ToList();
            
            // Assert
            Assert.Empty(vehicles);
        }
    }
} 