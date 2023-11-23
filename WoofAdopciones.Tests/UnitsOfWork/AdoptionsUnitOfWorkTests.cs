using Moq;
using WoofAdopciones.Backend.Repositories;
using WoofAdopciones.Backend.UnitsOfWork;
using WoofAdopciones.Shared.DTOs;
using WoofAdopciones.Shared.Entities;
using WoofAdopciones.Shared.Responses;

namespace WoofAdopciones.Tests.UnitsOfWork
{
    [TestClass]
    public class AdoptionsUnitOfWorkTests
    {
        private Mock<IGenericRepository<Adoption>> _repositoryMock = null!;
        private Mock<IAdoptionsRepository> _adoptionsRepositoryMock = null!;
        private AdoptionsUnitOfWork _unitOfWork = null!;

        [TestInitialize]
        public void SetUp()
        {
            _repositoryMock = new Mock<IGenericRepository<Adoption>>();
            _adoptionsRepositoryMock = new Mock<IAdoptionsRepository>();
            _unitOfWork = new AdoptionsUnitOfWork(_repositoryMock.Object, _adoptionsRepositoryMock.Object);
        }

        [TestMethod]
        public async Task GetAsync_WithPagination_ReturnsAdoptions()
        {
            // Arrange
            var pagination = new PaginationDTO();
            var expectedResponse = new Response<IEnumerable<Adoption>> { WasSuccess = true };
            _adoptionsRepositoryMock.Setup(x => x.GetAsync("email", pagination))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.GetAsync("email", pagination);

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _adoptionsRepositoryMock.Verify(x => x.GetAsync("email", pagination), Times.Once);
        }

        [TestMethod]
        public async Task GetTotalPagesAsync_ReturnsTotalPages()
        {
            // Arrange
            var pagination = new PaginationDTO();
            var expectedResponse = new Response<int> { WasSuccess = true };
            _adoptionsRepositoryMock.Setup(x => x.GetTotalPagesAsync("email", pagination))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.GetTotalPagesAsync("email", pagination);

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _adoptionsRepositoryMock.Verify(x => x.GetTotalPagesAsync("email", pagination), Times.Once);
        }

        [TestMethod]
        public async Task GetAsync_ById_ReturnsAdoption()
        {
            // Arrange
            var adoptionId = 1;
            var expectedResponse = new Response<Adoption> { WasSuccess = true };
            _adoptionsRepositoryMock.Setup(x => x.GetAsync(adoptionId))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.GetAsync(adoptionId);

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _adoptionsRepositoryMock.Verify(x => x.GetAsync(adoptionId), Times.Once);
        }

        [TestMethod]
        public async Task UpdateFullAsync_ReturnsAdoption()
        {
            // Arrange
            var adoptionDTO = new AdoptionDTO();
            var expectedResponse = new Response<Adoption> { WasSuccess = true };
            _adoptionsRepositoryMock.Setup(x => x.UpdateFullAsync("email", adoptionDTO))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.UpdateFullAsync("email", adoptionDTO);

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _adoptionsRepositoryMock.Verify(x => x.UpdateFullAsync( "email", adoptionDTO), Times.Once);
        }

        [TestMethod]
        public async Task ProcessAdoptionAsync_ReturnsTrue()
        {
            // Arrange
            var expectedResponse = new Response<bool> { WasSuccess = true };
            _adoptionsRepositoryMock.Setup(x => x.ProcessAdoptionAsync("email", 12))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.ProcessAdoptionAsync("email", 12);

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _adoptionsRepositoryMock.Verify(x => x.ProcessAdoptionAsync("email", 12), Times.Once);
        }
    }
}
