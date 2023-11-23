using Microsoft.EntityFrameworkCore;
using Moq;
using WoofAdopciones.Backend.Data;
using WoofAdopciones.Backend.Helpers;
using WoofAdopciones.Backend.Repositories;
using WoofAdopciones.Backend.UnitsOfWork;
using WoofAdopciones.Shared.DTOs;
using WoofAdopciones.Shared.Entities;
using WoofAdopciones.Shared.Enums;
using WoofAdopciones.Shared.Responses;

namespace WoofAdopciones.Tests.Repositories
{
    [TestClass]
    public class AdoptionsRepositoryTests
    {
        private DataContext _context = null!;
        private AdoptionsRepository _repository = null!;
        private Mock<IUserHelper> _mockUserHelper = null!;
        private Mock<IPetsUnitOfWork> _mockPetsUnitOfWork = null!;

        private DbContextOptions<DataContext> _options = null!;

        [TestInitialize]
        public void SetUp()
        {
            var _options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new DataContext(_options);
            _mockUserHelper = new Mock<IUserHelper>();
            _mockPetsUnitOfWork = new Mock<IPetsUnitOfWork>();
            _repository = new AdoptionsRepository(_context, _mockUserHelper.Object, _mockPetsUnitOfWork.Object);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Dispose();
        }

        [TestMethod]
        public async Task GetAsync_UserDoesNotExist_ReturnsFailedResponse()
        {
            // Act
            var response = await _repository.GetAsync("nonexistentuser@example.com", new PaginationDTO());

            // Assert
            Assert.IsFalse(response.WasSuccess);
            Assert.AreEqual("Usuario no válido", response.Message);
        }

        [TestMethod]
        public async Task GetAsync_ValidUserAndAdoption_ReturnsAdoption()
        {
            // Arrange
            var email = "test@example.com";
            var user = await CreateTestUser(email, UserType.User);
            await CreateTestAdoption(user);
            _mockUserHelper.Setup(x => x.GetUserAsync(email))
                .ReturnsAsync(user);
            _mockUserHelper.Setup(x => x.IsUserInRoleAsync(user, UserType.Admin.ToString()))
                .ReturnsAsync(false);

            // Act
            var response = await _repository.GetAsync(email, new PaginationDTO());

            // Assert
            Assert.IsTrue(response.WasSuccess);
            Assert.IsNotNull(response.Result);
            _mockUserHelper.Verify(x => x.GetUserAsync(email), Times.Once());
            _mockUserHelper.Verify(x => x.IsUserInRoleAsync(user, UserType.Admin.ToString()), Times.Once());
        }

        [TestMethod]
        public async Task GetTotalPagesAsync_UserDoesNotExist_ReturnsFailedResponse()
        {
            // Act
            var response = await _repository.GetTotalPagesAsync("nonexistentuser@example.com", new PaginationDTO());

            // Assert
            Assert.IsFalse(response.WasSuccess);
            Assert.AreEqual("Usuario no válido", response.Message);
        }

        [TestMethod]
        public async Task GetTotalPagesAsync_ReturnsCorrectNumberOfPages()
        {
            // Arrange
            var email = "test@example.com";
            var user = await CreateTestUser(email, UserType.User);
            await CreateTestAdoption(user);
            _mockUserHelper.Setup(x => x.GetUserAsync(email))
                .ReturnsAsync(user);
            _mockUserHelper.Setup(x => x.IsUserInRoleAsync(user, UserType.Admin.ToString()))
                .ReturnsAsync(false);
            var pagination = new PaginationDTO { RecordsNumber = 2, Page = 1 };

            // Act
            var response = await _repository.GetTotalPagesAsync(email, pagination);

            // Assert
            Assert.IsTrue(response.WasSuccess);
            Assert.AreEqual(1, response.Result);
            _mockUserHelper.Verify(x => x.GetUserAsync(email), Times.Once());
            _mockUserHelper.Verify(x => x.IsUserInRoleAsync(user, UserType.Admin.ToString()), Times.Once());
        }

        [TestMethod]
        public async Task UpdateFullAsync_UserDoesNotExist_ReturnsFailedResponse()
        {
            // Arrange
            var adoptionDTO = new AdoptionDTO { Id = 1 };

            // Act
            var response = await _repository.UpdateFullAsync("nonexistentuser@example.com", adoptionDTO);

            // Assert
            Assert.IsFalse(response.WasSuccess);
            Assert.AreEqual("Usuario no existe", response.Message);
        }

        [TestMethod]
        public async Task UpdateFullAsync_AdptionDoesNotExist_ReturnsFailedResponse()
        {
            // Arrange
            var email = "test@example.com";
            var user = await CreateTestUser(email, UserType.User);
            _mockUserHelper.Setup(x => x.GetUserAsync(email))
                .ReturnsAsync(user);
            _mockUserHelper.Setup(x => x.IsUserInRoleAsync(user, UserType.Admin.ToString()))
                .ReturnsAsync(true);
            var adoptionDTO = new AdoptionDTO { Id = 999 };

            // Act
            var response = await _repository.UpdateFullAsync(email, adoptionDTO);

            // Assert
            Assert.IsFalse(response.WasSuccess);
            Assert.AreEqual("Adopción no existe", response.Message);
            _mockUserHelper.Verify(x => x.GetUserAsync(email), Times.Once());
            _mockUserHelper.Verify(x => x.IsUserInRoleAsync(user, UserType.Admin.ToString()), Times.Once());
        }

        [TestMethod]
        public async Task UpdateFullAsync_ValidData_UpdatesOrder()
        {
            // Arrange
            var email = "admin@example.com";
            var user = await CreateTestUser(email, UserType.Admin);
            var order = await CreateTestAdoption(user);
            _mockUserHelper.Setup(x => x.GetUserAsync(email))
                .ReturnsAsync(user);
            _mockUserHelper.Setup(x => x.IsUserInRoleAsync(user, UserType.Admin.ToString()))
                .ReturnsAsync(true);
            var orderDTO = new AdoptionDTO { Id = order.Id };

            // Act
            var response = await _repository.UpdateFullAsync(email, orderDTO);

            // Assert
            Assert.IsTrue(response.WasSuccess);
            _mockUserHelper.Verify(x => x.GetUserAsync(email), Times.Once());
            _mockUserHelper.Verify(x => x.IsUserInRoleAsync(user, UserType.Admin.ToString()), Times.Once());
        }

        [TestMethod]
        public async Task ProcessAdoptionAsync_UserDoesNotExist_ReturnsFailedResponse()
        {
            // Arrange
            var petId = 1;

            // Act
            var response = await _repository.ProcessAdoptionAsync("nonexistentuser@example.com", petId);

            // Assert
            Assert.IsFalse(response.WasSuccess);
            Assert.AreEqual("Usuario no válido", response.Message);
        }

        [TestMethod]
        public async Task ProcessAdoptionAsync_PetDoesNotExist_ReturnsFailedResponse()
        {
            // Arrange
            var expectedResponse = new Response<Pet> { };

            var email = "test@example.com";
            var petId = 1;
            var user = await CreateTestUser(email, UserType.User);
            _mockUserHelper.Setup(x => x.GetUserAsync(email))
                .ReturnsAsync(user);
            _mockUserHelper.Setup(x => x.IsUserInRoleAsync(user, UserType.Admin.ToString()))
                .ReturnsAsync(true);
            _mockPetsUnitOfWork.Setup(x => x.GetAsync(petId)).ReturnsAsync(expectedResponse);

            // Act
            var response = await _repository.ProcessAdoptionAsync(email, 1);

            // Assert
            Assert.IsFalse(response.WasSuccess);
            Assert.AreEqual("No se encontró la mascota", response.Message);
        }

        [TestMethod]
        public async Task ProcessAdoptionAsync_ReturnsSuccessResponse()
        {
            // Arrange
            var petId = 1;
            var expectedResponse = new Response<Pet> { Result = new Pet { Id = petId } };

            var email = "test@example.com";
            
            var user = await CreateTestUser(email, UserType.User);
            _mockUserHelper.Setup(x => x.GetUserAsync(email))
                .ReturnsAsync(user);
            _mockUserHelper.Setup(x => x.IsUserInRoleAsync(user, UserType.Admin.ToString()))
                .ReturnsAsync(true);
            _mockPetsUnitOfWork.Setup(x => x.GetAsync(petId)).ReturnsAsync(expectedResponse);

            // Act
            var response = await _repository.ProcessAdoptionAsync(email, 1);

            // Assert
            Assert.IsNull(response);
        }

        private async Task<User> CreateTestUser(string email, UserType userType)
        {
            var user = new User { Email = email, UserType = userType, Address = "Any", Document = "Any", FirstName = "John", LastName = "Doe" };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        private async Task<Adoption> CreateTestAdoption(User user)
        {
            var order = new Adoption { User = user };
            await _context.Adoptions.AddAsync(order);
            await _context.SaveChangesAsync();
            return order;
        }
    }
}
