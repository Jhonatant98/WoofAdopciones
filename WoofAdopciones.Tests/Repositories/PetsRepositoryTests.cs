using Microsoft.EntityFrameworkCore;
using Moq;
using WoofAdopciones.Backend.Data;
using WoofAdopciones.Backend.Helpers;
using WoofAdopciones.Backend.Repositories;
using WoofAdopciones.Shared.DTOs;
using WoofAdopciones.Shared.Entities;
using WoofAdopciones.Tests.Shared;

namespace WoofAdopciones.Tests.Repositories
{
    [TestClass]
    public class petsRepositoryTests
    {
        private DataContext _context = null!;
        private PetsRepository _repository = null!;
        private Mock<IFileStorage> _fileStorageMock = null!;
        private DbContextOptions<DataContext> _options = null!;

        private const string _string64base = "U29tZVZhbGlkQmFzZTY0U3RyaW5n";
        private const string _container = "pets";

        [TestInitialize]
        public void SetUp()
        {
            _options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new DataContext(_options);
            _fileStorageMock = new Mock<IFileStorage>();
            _repository = new PetsRepository(_context, _fileStorageMock.Object);

            PopulateData();
        }

        [TestCleanup]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [TestMethod]
        public async Task AddImagesAsync_petNotFound_ReturnsError()
        {
            // Arrange
            var imageDto = new ImageDTO { PetId = 999 };

            // Act
            var result = await _repository.AddImageAsync(imageDto);

            // Assert
            Assert.IsFalse(result.WasSuccess);
        }

        [TestMethod]
        public async Task RemoveLastImageAsync_petNotFound_ReturnsError()
        {
            // Arrange
            var imageDto = new ImageDTO { PetId = 999 };

            // Act
            var result = await _repository.RemoveLastImageAsync(imageDto);

            // Assert
            Assert.IsFalse(result.WasSuccess);
        }

        [TestMethod]
        public async Task RemoveLastImageAsync_NoImages_ReturnsOk()
        {
            // Arrange
            var imageDto = new ImageDTO { PetId = 1 };

            // Act
            var result = await _repository.RemoveLastImageAsync(imageDto);

            // Assert
            Assert.IsTrue(result.WasSuccess);
        }

        [TestMethod]
        public async Task GetAsync_WithoutFilter_ReturnsAllpets()
        {
            // Arrange
            var pagination = new PaginationDTO { RecordsNumber = 10, Page = 1 };

            // Act
            var result = await _repository.GetAsync(pagination);

            // Assert
            Assert.IsTrue(result.WasSuccess);
            var pets = result.Result as List<Pet>;
            Assert.AreEqual(2, pets!.Count);
        }

        [TestMethod]
        public async Task GetAsync_WithPagination_Returnspets()
        {
            // Arrange
            var pagination = new PaginationDTO { Filter = "Some" };

            // Act
            var result = await _repository.GetAsync(pagination);

            // Assert
            Assert.IsTrue(result.WasSuccess);
        }

        [TestMethod]
        public async Task GetTotalPagesAsync_ReturnsTotalPages()
        {
            // Arrange
            var pagination = new PaginationDTO { Filter = "Some" };

            // Act
            var result = await _repository.GetTotalPagesAsync(pagination);

            // Assert
            Assert.IsTrue(result.WasSuccess);
        }

        [TestMethod]
        public async Task GetAsync_ValidId_Returnspet()
        {
            // Act
            var result = await _repository.GetAsync(1);

            // Assert
            Assert.IsTrue(result.WasSuccess);
            Assert.AreEqual("pet A", result.Result!.Name);
        }

        [TestMethod]
        public async Task GetAsync_InvalidId_ReturnsError()
        {
            // Act
            var result = await _repository.GetAsync(999);

            // Assert
            Assert.IsFalse(result.WasSuccess);
        }

      //  [TestMethod]
        public async Task AddFullAsync_ValidDTO_ReturnsOk()
        {
            // Arrange
            _fileStorageMock.Setup(fs => fs.SaveFileAsync(It.IsAny<byte[]>(), ".jpg", _container))
                .ReturnsAsync("testImage.jpg");

            var petDTO = new PetDTO
            {
                Name = "Testpet",
                Description = "Description",
                PetImages = new List<string> { _string64base },
            };

            // Act
            var result = await _repository.AddFullAsync(petDTO);

            // Assert
            Assert.IsTrue(result.WasSuccess);
            Assert.AreEqual("Testpet", result.Result!.Name);
            _fileStorageMock.Verify(x => x.SaveFileAsync(It.IsAny<byte[]>(), ".jpg", _container), Times.Once());
        }

        [TestMethod]
        public async Task AddFullAsync_DuplicateName_ReturnsErrors()
        {
            // Arrange
            var petDTO = new PetDTO
            {
                Name = "pet A",
                Description = "pet A",
                PetImages = new List<string> { _string64base },
            };

            // Act
            var result = await _repository.AddFullAsync(petDTO);

            // Assert
            Assert.IsFalse(result.WasSuccess);
            Assert.AreEqual("Ya existe una mascota con el mismo nombre.", result.Message);
        }

       // [TestMethod]
        public async Task AddFullAsync_GeneralException_ReturnsErrors()
        {
            // Arrange
            var petDTO = new PetDTO
            {
                Name = "pet A",
                Description = "pet A",
                PetImages = new List<string> { _string64base },
            };

            var message = "Test exception";
            _fileStorageMock.Setup(fs => fs.SaveFileAsync(It.IsAny<byte[]>(), ".jpg", _container))
                .Throws(new Exception(message));

            // Act
            var result = await _repository.AddFullAsync(petDTO);

            // Assert
            Assert.IsFalse(result.WasSuccess);
            Assert.AreEqual(message, result.Message);
            _fileStorageMock.Verify(x => x.SaveFileAsync(It.IsAny<byte[]>(), ".jpg", _container), Times.Once());
        }

        [TestMethod]
        public async Task UpdateFullAsync_ValidDTO_Updatespet()
        {
            // Arrange

            var petDTO = new PetDTO
            {
                Id = 1,
                Name = "NewName",
                Description = "NewDescription"
            };

            // Act
            var result = await _repository.UpdateFullAsync(petDTO);
        }

        [TestMethod]
        public async Task UpdateFullAsync_NonExistingpet_ReturnsError()
        {
            // Arrange
            var petDTO = new PetDTO
            {
                Id = 999,
                Name = "TestName",
                Description = "TestDescription"
            };

            // Act
            var result = await _repository.UpdateFullAsync(petDTO);

            // Assert
            Assert.IsFalse(result.WasSuccess);
        }

        [TestMethod]
        public async Task UpdateFullAsync_GeneralException_ReturnsError()
        {
            // Arrange
            var exceptionalContext = new ExceptionalDataContext(_options);

            exceptionalContext.Pets.Add(new Pet { Id = 1, Name = "OriginalName", Description = "Description" });
            exceptionalContext.Pets.Add(new Pet { Id = 2, Name = "DuplicateName", Description = "Description" });
            exceptionalContext.SaveChanges();

            var repository = new PetsRepository(exceptionalContext, _fileStorageMock.Object);

            var petDTO = new PetDTO
            {
                Id = 1,
                Name = "DuplicateName",
                Description = "Description"
            };

            // Act
            var result = await repository.UpdateFullAsync(petDTO);

            // Assert
            Assert.IsFalse(result.WasSuccess);
            Assert.AreEqual("Test Exception", result.Message);
        }

        [TestMethod]
        public async Task UpdateFullAsync_DbUpdateException_ReturnsError()
        {
            // Arrange
            var exceptionalContext = new ExceptionalDBUpdateDataContext(_options);

            exceptionalContext.Pets.Add(new Pet { Id = 1, Name = "OriginalName", Description = "Description" });
            exceptionalContext.Pets.Add(new Pet { Id = 2, Name = "DuplicateName", Description = "Description" });
            exceptionalContext.SaveChanges();

            var repository = new PetsRepository(exceptionalContext, _fileStorageMock.Object);

            var petDTO = new PetDTO
            {
                Id = 1,
                Name = "DuplicateName",
                Description = "Description"
            };

            // Act
            var result = await repository.UpdateFullAsync(petDTO);

            // Assert
            Assert.IsFalse(result.WasSuccess);
            Assert.AreEqual("Ya existe una mascota con el mismo nombre.", result.Message);
        }

        private void PopulateData()
        {

            var pet1 = new Pet
            {
                Id = 1,
                Name = "pet A",
                Description = "pet A",
            };
            var pet2 = new Pet
            {
                Id = 2,
                Name = "pet B",
                Description = "pet B",
                PetImages = new List<PetImage>
                {
                    new PetImage { Image = "https//image1.jpg" },
                    new PetImage { Image = "https//image2.jpg" }
                }
            };
            _context.Pets.AddRange(pet1, pet2);
            _context.SaveChanges();
        }
    }
}
