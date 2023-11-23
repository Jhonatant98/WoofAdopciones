using Moq;
using WoofAdopciones.Backend.Repositories;
using WoofAdopciones.Backend.UnitsOfWork;
using WoofAdopciones.Shared.DTOs;
using WoofAdopciones.Shared.Entities;
using WoofAdopciones.Shared.Responses;

namespace WoofAdopciones.Tests.UnitsOfWork
{
   [TestClass]
    public class PetsUnitOfWorkTests
    {
        private Mock<IGenericRepository<Pet>> _repositoryMock = null!;
        private Mock<IPetsRepository> _petsRepositoryMock = null!;
        private PetsUnitOfWork _unitOfWork = null!;

        [TestInitialize]
        public void SetUp()
        {
            _repositoryMock = new Mock<IGenericRepository<Pet>>();
            _petsRepositoryMock = new Mock<IPetsRepository>();
            _unitOfWork = new PetsUnitOfWork(_repositoryMock.Object, _petsRepositoryMock.Object);
        }

        [TestMethod]
        public async Task GetAsync_WithPagination_ReturnsPets()
        {
            // Arrange
            var pagination = new PaginationDTO();
            var expectedResponse = new Response<IEnumerable<Pet>> { WasSuccess = true };
            _petsRepositoryMock.Setup(x => x.GetAsync(pagination))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.GetAsync(pagination);

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _petsRepositoryMock.Verify(x => x.GetAsync(pagination), Times.Once);
        }

        [TestMethod]
        public async Task GetTotalPagesAsync_ReturnsTotalPages()
        {
            // Arrange
            var pagination = new PaginationDTO();
            var expectedResponse = new Response<int> { WasSuccess = true };
            _petsRepositoryMock.Setup(x => x.GetTotalPagesAsync(pagination))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.GetTotalPagesAsync(pagination);

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _petsRepositoryMock.Verify(x => x.GetTotalPagesAsync(pagination), Times.Once);
        }

        [TestMethod]
        public async Task GetAsync_ById_ReturnsPet()
        {
            // Arrange
            var petId = 1;
            var expectedResponse = new Response<Pet> { WasSuccess = true };
            _petsRepositoryMock.Setup(x => x.GetAsync(petId))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.GetAsync(petId);

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _petsRepositoryMock.Verify(x => x.GetAsync(petId), Times.Once);
        }

        [TestMethod]
        public async Task AddFullAsync_ReturnsPet()
        {
            // Arrange
            var petDTO = new PetDTO();
            var expectedResponse = new Response<Pet> { WasSuccess = true };
            _petsRepositoryMock.Setup(x => x.AddFullAsync(petDTO))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.AddFullAsync(petDTO);

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _petsRepositoryMock.Verify(x => x.AddFullAsync(petDTO), Times.Once);
        }

        [TestMethod]
        public async Task UpdateFullAsync_ReturnsPet()
        {
            // Arrange
            var petDTO = new PetDTO();
            var expectedResponse = new Response<Pet> { WasSuccess = true };
            _petsRepositoryMock.Setup(x => x.UpdateFullAsync(petDTO))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.UpdateFullAsync(petDTO);

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _petsRepositoryMock.Verify(x => x.UpdateFullAsync(petDTO), Times.Once);
        }

        [TestMethod]
        public async Task AddImageAsync_ReturnsImage()
        {
            // Arrange
            var imageDTO = new ImageDTO();
            var expectedResponse = new Response<ImageDTO> { WasSuccess = true };
            _petsRepositoryMock.Setup(x => x.AddImageAsync(imageDTO))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.AddImageAsync(imageDTO);

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _petsRepositoryMock.Verify(x => x.AddImageAsync(imageDTO), Times.Once);
        }

        [TestMethod]
        public async Task RemoveLastImageAsync_ReturnsImage()
        {
            // Arrange
            var imageDTO = new ImageDTO();
            var expectedResponse = new Response<ImageDTO> { WasSuccess = true };
            _petsRepositoryMock.Setup(x => x.RemoveLastImageAsync(imageDTO))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.RemoveLastImageAsync(imageDTO);

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _petsRepositoryMock.Verify(x => x.RemoveLastImageAsync(imageDTO), Times.Once);
        }
    }
}
