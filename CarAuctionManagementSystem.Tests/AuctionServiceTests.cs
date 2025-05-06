using System;
using System.Linq;
using CarAuctionManagementSystem.Exceptions;
using CarAuctionManagementSystem.Models;
using CarAuctionManagementSystem.Services;
using Xunit;

namespace CarAuctionManagementSystem.Tests
{
    public class AuctionServiceTests
    {
        private readonly VehicleInventoryService _inventoryService;
        private readonly AuctionService _auctionService;
        private readonly Sedan _testSedan;
        private readonly SUV _testSuv;

        public AuctionServiceTests()
        {
            _inventoryService = new VehicleInventoryService();
            _auctionService = new AuctionService(_inventoryService);
            
            _testSedan = new Sedan("SED123", "Toyota", "Camry", 2020, 18000m, 4);
            _testSuv = new SUV("SUV123", "Ford", "Explorer", 2021, 35000m, 7);
            
            _inventoryService.AddVehicle(_testSedan);
            _inventoryService.AddVehicle(_testSuv);
        }

        [Fact]
        public void StartAuction_ValidVehicle_CreatesAuction()
        {
            // Arrange - done in constructor
            
            // Act
            var auction = _auctionService.StartAuction(_testSedan.Id);
            
            // Assert
            Assert.NotNull(auction);
            Assert.Equal(_testSedan, auction.Vehicle);
            Assert.True(auction.IsActive);
            Assert.Equal(_testSedan.StartingBid, auction.CurrentHighestBid);
        }

        [Fact]
        public void StartAuction_NonExistentVehicle_ThrowsVehicleNotFoundException()
        {
            // Arrange, Act & Assert
            Assert.Throws<VehicleNotFoundException>(() => _auctionService.StartAuction("cybertruck"));
        }

        [Fact]
        public void StartAuction_AlreadyActiveAuction_ThrowsAuctionAlreadyActiveException()
        {
            // Arrange
            _auctionService.StartAuction(_testSedan.Id);
            
            // Act & Assert
            Assert.Throws<AuctionAlreadyActiveException>(() => _auctionService.StartAuction(_testSedan.Id));
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public void StartAuction_InvalidVehicleId_ThrowsArgumentException(string vehicleId)
        {
            // Arrange, Act & Assert
            Assert.Throws<ArgumentException>(() => _auctionService.StartAuction(vehicleId));
        }

        [Fact]
        public void StartAuction_InvalidVehicleIdNull_ThrowsArgumentException()
        {
            // Arrange, Act & Assert
            Assert.Throws<ArgumentException>(() => _auctionService.StartAuction(null!));
        }

        [Fact]
        public void PlaceBid_ValidBid_UpdatesHighestBid()
        {
            // Arrange
            var auction = _auctionService.StartAuction(_testSedan.Id);
            var bidAmount = auction.CurrentHighestBid + 100m;
            
            // Act
            _auctionService.PlaceBid(_testSedan.Id, "Alex", bidAmount);
            
            // Assert
            var updatedAuction = _auctionService.GetActiveAuction(_testSedan.Id);
            Assert.NotNull(updatedAuction);
            Assert.Equal(bidAmount, updatedAuction.CurrentHighestBid);
            Assert.Equal("Alex", updatedAuction.CurrentHighestBidder);
            Assert.Single(updatedAuction.BidHistory);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public void PlaceBid_InvalidBidder_ThrowsArgumentException(string bidder)
        {
            // Arrange
            var auction = _auctionService.StartAuction(_testSedan.Id);
            var bidAmount = auction.CurrentHighestBid + 100m;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _auctionService.PlaceBid(_testSedan.Id, bidder, bidAmount));
        }

        [Fact]
        public void PlaceBid_InvalidBidderNull_ThrowsArgumentException()
        {
            // Arrange
            var auction = _auctionService.StartAuction(_testSedan.Id);
            var bidAmount = auction.CurrentHighestBid + 100m;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _auctionService.PlaceBid(_testSedan.Id, null!, bidAmount));
        }

        [Fact]
        public void PlaceBid_NoActiveAuction_ThrowsAuctionNotFoundException()
        {
            // Arrange, Act & Assert
            Assert.Throws<AuctionNotFoundException>(() => _auctionService.PlaceBid(_testSedan.Id, "Alex", 20000m));
        }

        [Fact]
        public void PlaceBid_BidTooLow_ThrowsInvalidBidException()
        {
            // Arrange
            var auction = _auctionService.StartAuction(_testSedan.Id);
            var lowBidAmount = auction.CurrentHighestBid - 100m;
            
            // Act & Assert
            Assert.Throws<InvalidBidException>(() => _auctionService.PlaceBid(_testSedan.Id, "Alex", lowBidAmount));
        }

        [Fact]
        public void PlaceBid_BidEqualToCurrent_ThrowsInvalidBidException()
        {
            // Arrange
            var auction = _auctionService.StartAuction(_testSedan.Id);
            
            // Act & Assert
            Assert.Throws<InvalidBidException>(() => _auctionService.PlaceBid(_testSedan.Id, "Alex", auction.CurrentHighestBid));
        }

        [Fact]
        public void CloseAuction_ValidAuction_ClosesAuction()
        {
            // Arrange
            _auctionService.StartAuction(_testSedan.Id);
            
            // Act
            var closedAuction = _auctionService.CloseAuction(_testSedan.Id);
            
            // Assert
            Assert.NotNull(closedAuction);
            Assert.False(closedAuction.IsActive);
            Assert.NotNull(closedAuction.EndTime);
        }

        [Fact]
        public void CloseAuction_NoActiveAuction_ThrowsAuctionNotFoundException()
        {
            // Arrange, Act & Assert
            Assert.Throws<AuctionNotFoundException>(() => _auctionService.CloseAuction(_testSedan.Id));
        }

        [Fact]
        public void GetActiveAuction_ExistingActiveAuction_ReturnsAuction()
        {
            // Arrange
            var auction = _auctionService.StartAuction(_testSedan.Id);
            
            // Act
            var activeAuction = _auctionService.GetActiveAuction(_testSedan.Id);
            
            // Assert
            Assert.NotNull(activeAuction);
            Assert.Equal(auction.Id, activeAuction.Id);
            Assert.Equal(_testSedan.Id, activeAuction.Vehicle.Id);
        }

        [Fact]
        public void GetActiveAuction_NoActiveAuction_ReturnsNull()
        {
            // Arrange, Act
            var activeAuction = _auctionService.GetActiveAuction(_testSedan.Id);
            
            // Assert
            Assert.Null(activeAuction);
        }

        [Fact]
        public void GetActiveAuction_ClosedAuction_ReturnsNull()
        {
            // Arrange
            _auctionService.StartAuction(_testSedan.Id);
            _auctionService.CloseAuction(_testSedan.Id);
            
            // Act
            var activeAuction = _auctionService.GetActiveAuction(_testSedan.Id);
            
            // Assert
            Assert.Null(activeAuction);
        }

        [Fact]
        public void GetAllActiveAuctions_MultipleActiveAuctions_ReturnsAllActiveAuctions()
        {
            // Arrange
            _auctionService.StartAuction(_testSedan.Id);
            _auctionService.StartAuction(_testSuv.Id);
            
            // Act
            var activeAuctions = _auctionService.GetAllActiveAuctions().ToList();
            
            // Assert
            Assert.Equal(2, activeAuctions.Count);
            Assert.Contains(activeAuctions, a => a.Vehicle.Id == _testSedan.Id);
            Assert.Contains(activeAuctions, a => a.Vehicle.Id == _testSuv.Id);
        }

        [Fact]
        public void GetAllActiveAuctions_NoActiveAuctions_ReturnsEmptyList()
        {
            // Arrange, Act
            var activeAuctions = _auctionService.GetAllActiveAuctions().ToList();
            
            // Assert
            Assert.Empty(activeAuctions);
        }

        [Fact]
        public void GetAllActiveAuctions_MixOfActiveAndClosedAuctions_ReturnsOnlyActiveAuctions()
        {
            // Arrange
            _auctionService.StartAuction(_testSedan.Id);
            _auctionService.StartAuction(_testSuv.Id);
            _auctionService.CloseAuction(_testSedan.Id);
            
            // Act
            var activeAuctions = _auctionService.GetAllActiveAuctions().ToList();
            
            // Assert
            Assert.Single(activeAuctions);
            Assert.Equal(_testSuv.Id, activeAuctions.First().Vehicle.Id);
        }

        [Fact]
        public void GetAuctionHistory_VehicleWithAuctions_ReturnsAllAuctions()
        {
            // Arrange
            _auctionService.StartAuction(_testSedan.Id);
            _auctionService.CloseAuction(_testSedan.Id);
            _auctionService.StartAuction(_testSedan.Id);
            
            // Act
            var auctionHistory = _auctionService.GetAuctionHistory(_testSedan.Id).ToList();
            
            // Assert
            Assert.Equal(2, auctionHistory.Count);
            Assert.Contains(auctionHistory, a => !a.IsActive);
            Assert.Contains(auctionHistory, a => a.IsActive);
        }

        [Fact]
        public void GetAuctionHistory_VehicleWithNoAuctions_ReturnsEmptyList()
        {
            // Arrange, Act
            var auctionHistory = _auctionService.GetAuctionHistory(_testSedan.Id).ToList();
            
            // Assert
            Assert.Empty(auctionHistory);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public void GetAuctionHistory_InvalidVehicleId_ThrowsArgumentException(string vehicleId)
        {
            // Arrange, Act & Assert
            Assert.Throws<ArgumentException>(() => _auctionService.GetAuctionHistory(vehicleId));
        }

        [Fact]
        public void GetAuctionHistory_InvalidVehicleIdNull_ThrowsArgumentException()
        {
            // Arrange, Act & Assert
            Assert.Throws<ArgumentException>(() => _auctionService.GetAuctionHistory(null!));
        }
    }
} 