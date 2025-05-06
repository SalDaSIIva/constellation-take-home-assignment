using System;
using CarAuctionManagementSystem.Models;
using Xunit;

namespace CarAuctionManagementSystem.Tests
{
    public class VehicleTests
    {
        [Fact]
        public void Sedan_Creation_Success()
        {
            // Arrange & Act
            var sedan = new Sedan("SED123", "Toyota", "Camry", 2020, 18000m, 4);
            
            // Assert
            Assert.Equal("SED123", sedan.Id);
            Assert.Equal("Toyota", sedan.Manufacturer);
            Assert.Equal("Camry", sedan.Model);
            Assert.Equal(2020, sedan.Year);
            Assert.Equal(18000m, sedan.StartingBid);
            Assert.Equal(4, sedan.NumberOfDoors);
            Assert.Equal(VehicleType.Sedan, sedan.Type);
        }

        [Theory]
        [InlineData("", "Vehicle ID cannot be empty")]
        [InlineData("   ", "Vehicle ID cannot be empty")]
        public void Sedan_Invalid_Id_Throws_Exception(string id, string expectedExceptionSubstring)
        {
            // Arrange, Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => 
                new Sedan(id, "Toyota", "Camry", 2020, 18000m, 4));
            
            Assert.Contains(expectedExceptionSubstring, exception.Message);
        }

        [Theory]
        [InlineData("", "Manufacturer cannot be empty")]
        [InlineData("   ", "Manufacturer cannot be empty")]
        public void Sedan_Invalid_Manufacturer_Throws_Exception(string manufacturer, string expectedExceptionSubstring)
        {
            // Arrange, Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => 
                new Sedan("SED123", manufacturer, "Camry", 2020, 18000m, 4));
            
            Assert.Contains(expectedExceptionSubstring, exception.Message);
        }

        [Theory]
        [InlineData("", "Model cannot be empty")]
        [InlineData("   ", "Model cannot be empty")]
        public void Sedan_Invalid_Model_Throws_Exception(string model, string expectedExceptionSubstring)
        {
            // Arrange, Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => 
                new Sedan("SED123", "Toyota", model, 2020, 18000m, 4));
            
            Assert.Contains(expectedExceptionSubstring, exception.Message);
        }

        [Theory]
        [InlineData(1800, "Year must be between")]
        [InlineData(2100, "Year must be between")]
        public void Sedan_Invalid_Year_Throws_Exception(int year, string expectedExceptionSubstring)
        {
            // Arrange, Act & Assert
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() => 
                new Sedan("SED123", "Toyota", "Camry", year, 18000m, 4));
            
            Assert.Contains(expectedExceptionSubstring, exception.Message);
        }

        [Theory]
        [InlineData(0, "Starting bid must be greater than zero")]
        [InlineData(-1, "Starting bid must be greater than zero")]
        public void Sedan_Invalid_StartingBid_Throws_Exception(decimal startingBid, string expectedExceptionSubstring)
        {
            // Arrange, Act & Assert
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() => 
                new Sedan("SED123", "Toyota", "Camry", 2020, startingBid, 4));
            
            Assert.Contains(expectedExceptionSubstring, exception.Message);
        }

        [Theory]
        [InlineData(1, "Number of doors must be between")]
        [InlineData(6, "Number of doors must be between")]
        public void Sedan_Invalid_NumberOfDoors_Throws_Exception(int numberOfDoors, string expectedExceptionSubstring)
        {
            // Arrange, Act & Assert
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() => 
                new Sedan("SED123", "Toyota", "Camry", 2020, 18000m, numberOfDoors));
            
            Assert.Contains(expectedExceptionSubstring, exception.Message);
        }

        [Fact]
        public void SUV_Creation_Success()
        {
            // Arrange & Act
            var suv = new SUV("SUV123", "Ford", "Explorer", 2021, 35000m, 7);
            
            // Assert
            Assert.Equal("SUV123", suv.Id);
            Assert.Equal("Ford", suv.Manufacturer);
            Assert.Equal("Explorer", suv.Model);
            Assert.Equal(2021, suv.Year);
            Assert.Equal(35000m, suv.StartingBid);
            Assert.Equal(7, suv.NumberOfSeats);
            Assert.Equal(VehicleType.SUV, suv.Type);
        }

        [Theory]
        [InlineData(1, "Number of seats must be between")]
        [InlineData(10, "Number of seats must be between")]
        public void SUV_Invalid_NumberOfSeats_Throws_Exception(int numberOfSeats, string expectedExceptionSubstring)
        {
            // Arrange, Act & Assert
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() => 
                new SUV("SUV123", "Ford", "Explorer", 2021, 35000m, numberOfSeats));
            
            Assert.Contains(expectedExceptionSubstring, exception.Message);
        }

        [Fact]
        public void Truck_Creation_Success()
        {
            // Arrange & Act
            var truck = new Truck("TRK123", "Chevrolet", "Silverado", 2020, 32000m, 2.5m);
            
            // Assert
            Assert.Equal("TRK123", truck.Id);
            Assert.Equal("Chevrolet", truck.Manufacturer);
            Assert.Equal("Silverado", truck.Model);
            Assert.Equal(2020, truck.Year);
            Assert.Equal(32000m, truck.StartingBid);
            Assert.Equal(2.5m, truck.LoadCapacity);
            Assert.Equal(VehicleType.Truck, truck.Type);
        }

        [Theory]
        [InlineData(0, "Load capacity must be greater than zero")]
        [InlineData(-1, "Load capacity must be greater than zero")]
        public void Truck_Invalid_LoadCapacity_Throws_Exception(decimal loadCapacity, string expectedExceptionSubstring)
        {
            // Arrange, Act & Assert
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() => 
                new Truck("TRK123", "Chevrolet", "Silverado", 2020, 32000m, loadCapacity));
            
            Assert.Contains(expectedExceptionSubstring, exception.Message);
        }

        [Fact]
        public void Hatchback_Creation_Success()
        {
            // Arrange & Act
            var hatchback = new Hatchback("HAT123", "Volkswagen", "Golf", 2019, 16000m, 5);
            
            // Assert
            Assert.Equal("HAT123", hatchback.Id);
            Assert.Equal("Volkswagen", hatchback.Manufacturer);
            Assert.Equal("Golf", hatchback.Model);
            Assert.Equal(2019, hatchback.Year);
            Assert.Equal(16000m, hatchback.StartingBid);
            Assert.Equal(5, hatchback.NumberOfDoors);
            Assert.Equal(VehicleType.Hatchback, hatchback.Type);
        }

        [Theory]
        [InlineData(1, "Number of doors must be between")]
        [InlineData(6, "Number of doors must be between")]
        public void Hatchback_Invalid_NumberOfDoors_Throws_Exception(int numberOfDoors, string expectedExceptionSubstring)
        {
            // Arrange, Act & Assert
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() => 
                new Hatchback("HAT123", "Volkswagen", "Golf", 2019, 16000m, numberOfDoors));
            
            Assert.Contains(expectedExceptionSubstring, exception.Message);
        }
    }
} 