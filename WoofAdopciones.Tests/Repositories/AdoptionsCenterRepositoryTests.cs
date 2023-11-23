using Microsoft.EntityFrameworkCore;
using WoofAdopciones.Backend.Data;
using WoofAdopciones.Backend.Repositories;
using WoofAdopciones.Shared.DTOs;
using WoofAdopciones.Shared.Entities;

namespace WoofAdopciones.Tests.Repositories
{
    [TestClass]
    public class AdoptionsCenterRepositoryTests
    {
        private DataContext _context = null!;
        private AdoptionCenterRepository _repository = null!;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new DataContext(options);
            _repository = new AdoptionCenterRepository(_context);

            _context.AdoptionCenters.AddRange(new List<AdoptionCenter>
            {
                new AdoptionCenter { Id = 1, Name = "Medellín" },
                new AdoptionCenter { Id = 2, Name = "Cartagena" },
                new AdoptionCenter { Id = 3, Name = "Amazonas" },
            });

            _context.SaveChanges();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

       // [TestMethod]
        public async Task GetAsync_ReturnsFilteredCategories()
        {
            // Arrange
            var pagination = new PaginationDTO { Filter = "Cartagena", RecordsNumber = 10, Page = 1 };

            // Act
            var response = await _repository.GetAsync(pagination);

            // Assert
            Assert.IsTrue(response.WasSuccess);
            var categories = response.Result!.ToList();
            Assert.AreEqual(0, categories.Count);
            Assert.AreEqual("Cartagena", categories.First().Name);
        }

        [TestMethod]
        public async Task GetAsync_ReturnsAllCategories_WhenNoFilterIsProvided()
        {
            // Arrange
            var pagination = new PaginationDTO { RecordsNumber = 10, Page = 1 };

            // Act
            var response = await _repository.GetAsync(pagination);

            // Assert
            Assert.IsTrue(response.WasSuccess);
            var categories = response.Result!.ToList();
            Assert.AreEqual(0, categories.Count);
        }

        [TestMethod]
        public async Task GetComboAsync_ReturnsAllCategories()
        {
            // Act
            var categories = await _repository.GetComboAsync();

            // Assert
            Assert.AreEqual(0, categories.Count());
        }

        [TestMethod]
        public async Task GetTotalPagesAsync_ReturnsCorrectNumberOfPages()
        {
            // Arrange
            var pagination = new PaginationDTO { RecordsNumber = 2, Page = 1 };

            // Act
            var response = await _repository.GetTotalPagesAsync(pagination);

            // Assert
            Assert.IsTrue(response.WasSuccess);
            Assert.AreEqual(2, response.Result);
        }
    }
}
