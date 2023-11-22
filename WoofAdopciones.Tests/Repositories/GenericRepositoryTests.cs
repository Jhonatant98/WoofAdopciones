﻿using Microsoft.EntityFrameworkCore;
using WoofAdopciones.Backend.Data;
using WoofAdopciones.Backend.Repositories;
using WoofAdopciones.Shared.DTOs;
using WoofAdopciones.Shared.Entities;
using WoofAdopciones.Tests.Shared;

namespace WoofAdopciones.Tests.Repositories
{
    [TestClass]
    public class GenericRepositoryTests
    {
        private DataContext _context = null!;
        private DbContextOptions<DataContext> _options = null!;
        private GenericRepository<Pet> _repository = null!;

        [TestInitialize]
        public void Initialize()
        {
            _options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new DataContext(_options);
            _repository = new GenericRepository<Pet>(_context);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [TestMethod]
        public async Task AddAsync_ShouldAddEntity()
        {
            // Arrange
            var testEntity = new Pet { Name = "Test" };

            // Act
            var response = await _repository.AddAsync(testEntity);

            // Assert
            Assert.IsTrue(response.WasSuccess);
            Assert.IsNotNull(response.Result);
            Assert.AreEqual("Test", response.Result.Name);
        }

        [TestMethod]
        public async Task AddAsync_GeneralExceptionThrown_ReturnsError()
        {
            // Arrange
            var exceptionalContext = new ExceptionalDataContext(_options);
            var repository = new GenericRepository<Pet>(exceptionalContext);
            var testEntity = new Pet { Name = "Test" };

            // Act
            var response = await repository.AddAsync(testEntity);

            // Assert
            Assert.IsFalse(response.WasSuccess);
            Assert.AreEqual("Test Exception", response.Message);
        }

        [TestMethod]
        public async Task AddAsync_DbUpdateExceptionThrown_ReturnsError()
        {
            // Arrange
            var exceptionalContext = new ExceptionalDBUpdateDataContext(_options);
            var repository = new GenericRepository<Pet>(exceptionalContext);
            var testEntity = new Pet { Name = "Test" };

            // Act
            var response = await repository.AddAsync(testEntity);

            // Assert
            Assert.IsFalse(response.WasSuccess);
            Assert.AreEqual("Ya existe el registro que estas intentando crear.", response.Message);
        }

        [TestMethod]
        public async Task DeleteAsync_ShouldDeleteEntity()
        {
            // Arrange
            var testEntity = new Pet { Name = "Test" };
            await _context.Set<Pet>().AddAsync(testEntity);
            await _context.SaveChangesAsync();

            // Act
            var response = await _repository.DeleteAsync(testEntity.Id);

            // Assert
            Assert.IsTrue(response.WasSuccess);
        }

        [TestMethod]
        public async Task DeleteAsync_EntityNotFound_ShouldReturnErrorResponse()
        {
            // Act
            var response = await _repository.DeleteAsync(1);

            // Assert
            Assert.IsFalse(response.WasSuccess);
            Assert.AreEqual("Registro no encontrado", response.Message);
        }

        [TestMethod]
        public async Task GetAsync_ById_ShouldReturnEntity()
        {
            // Arrange
            var testEntity = new Pet { Name = "Test" };
            await _context.Set<Pet>().AddAsync(testEntity);
            await _context.SaveChangesAsync();

            // Act
            var response = await _repository.GetAsync(testEntity.Id);

            // Assert
            Assert.IsTrue(response.WasSuccess);
            Assert.IsNotNull(response.Result);
            Assert.AreEqual("Test", response.Result.Name);
        }

        [TestMethod]
        public async Task GetAsync_ById_EntityNotFound_ShouldReturnErrorResponse()
        {
            // Act
            var response = await _repository.GetAsync(1);

            // Assert
            Assert.IsFalse(response.WasSuccess);
            Assert.AreEqual("Registro no encontrado", response.Message);
        }

        [TestMethod]
        public async Task GetAsync_Pagination_ShouldReturnEntities()
        {
            // Arrange
            await _context.Set<Pet>().AddRangeAsync(new List<Pet>
            {
                new Pet { Name = "Test1" },
                new Pet { Name = "Test2" },
                new Pet { Name = "Test3" },
            });
            await _context.SaveChangesAsync();

            // Act
            var paginationDTO = new PaginationDTO { RecordsNumber = 2 };
            var response = await _repository.GetAsync(paginationDTO);

            // Assert
            Assert.IsTrue(response.WasSuccess);
            Assert.IsNotNull(response.Result);
            Assert.AreEqual(2, response.Result.Count());
        }

        [TestMethod]
        public async Task GetTotalPagesAsync_ShouldReturnTotalPages()
        {
            // Arrange
            await _context.Set<Pet>().AddRangeAsync(new List<Pet>
            {
                new Pet { Name = "Test1" },
                new Pet { Name = "Test2" },
                new Pet { Name = "Test3" },
            });
            await _context.SaveChangesAsync();
            var paginationDTO = new PaginationDTO { RecordsNumber = 2 };

            // Act
            var response = await _repository.GetTotalPagesAsync(paginationDTO);

            // Assert
            Assert.IsTrue(response.WasSuccess);
            Assert.AreEqual(2, response.Result);
        }

        [TestMethod]
        public async Task UpdateAsync_ShouldUpdateEntity()
        {
            // Arrange
            var testEntity = new Pet { Name = "Test" };
            await _context.Set<Pet>().AddAsync(testEntity);
            await _context.SaveChangesAsync();
            testEntity.Name = "UpdatedTest";

            // Act
            var response = await _repository.UpdateAsync(testEntity);

            // Assert
            Assert.IsTrue(response.WasSuccess);
            Assert.IsNotNull(response.Result);
            Assert.AreEqual("UpdatedTest", response.Result.Name);
        }

        [TestMethod]
        public async Task UpdateAsync_GeneralExceptionThrown_ReturnsError()
        {
            // Arrange
            var exceptionalContext = new ExceptionalDataContext(_options);
            var testEntity = new Pet { Name = "Test" };
            await exceptionalContext.Set<Pet>().AddAsync(testEntity);
            exceptionalContext.SaveChanges();
            var repository = new GenericRepository<Pet>(exceptionalContext);
            testEntity.Name = "UpdatedTest";

            // Act
            var response = await repository.UpdateAsync(testEntity);

            // Assert
            Assert.IsFalse(response.WasSuccess);
            Assert.AreEqual("Test Exception", response.Message);
        }

        [TestMethod]
        public async Task UpdateAsync_DbUpdateExceptionThrown_ReturnsError()
        {
            // Arrange
            var exceptionalContext = new ExceptionalDBUpdateDataContext(_options);
            var testEntity = new Pet { Name = "Test" };
            await exceptionalContext.Set<Pet>().AddAsync(testEntity);
            exceptionalContext.SaveChanges();
            var repository = new GenericRepository<Pet>(exceptionalContext);
            testEntity.Name = "UpdatedTest";

            // Act
            var response = await repository.UpdateAsync(testEntity);

            // Assert
            Assert.IsFalse(response.WasSuccess);
            Assert.AreEqual("Ya existe el registro que estas intentando crear.", response.Message);
        }
    }
}
