using Moq;
using WoofAdopciones.Backend.Repositories;
using WoofAdopciones.Backend.UnitsOfWork;
using WoofAdopciones.Shared.DTOs;
using WoofAdopciones.Shared.Entities;
using WoofAdopciones.Shared.Responses;

namespace WoofAdopciones.Tests.UnitsOfWork
{
    [TestClass]
    public class AdoptionCenterUnitOfWorkTests
    {
        private Mock<IGenericRepository<AdoptionCenter>> _mockGenericRepository = null!;
        private Mock<IAdoptionCenterRepository> _mockAdoptionCenterRepository = null!;
        private AdoptionCenterUnitOfWork _unitOfWork = null!;

        [TestInitialize]
        public void Setup()
        {
            _mockGenericRepository = new Mock<IGenericRepository<AdoptionCenter>>();
            _mockAdoptionCenterRepository = new Mock<IAdoptionCenterRepository>();
            _unitOfWork = new AdoptionCenterUnitOfWork(_mockGenericRepository.Object, _mockAdoptionCenterRepository.Object);
        }

        [TestMethod]
        public async Task GetAsync_CallsRepositoryAndReturnsResult()
        {
            // Arrange
            var pagination = new PaginationDTO();
            var expectedResponse = new Response<IEnumerable<AdoptionCenter>> { Result = new List<AdoptionCenter>() };
            _mockAdoptionCenterRepository.Setup(x => x.GetAsync(pagination)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.GetAsync(pagination);

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _mockAdoptionCenterRepository.Verify(x => x.GetAsync(pagination), Times.Once);
        }

        [TestMethod]
        public async Task GetComboAsync_CallsRepositoryAndReturnsResult()
        {
            // Arrange
            var expectedCategories = new List<AdoptionCenter> { new AdoptionCenter() };
            _mockAdoptionCenterRepository.Setup(x => x.GetComboAsync()).ReturnsAsync(expectedCategories);

            // Act
            var result = await _unitOfWork.GetComboAsync();

            // Assert
            Assert.AreEqual(expectedCategories, result);
            _mockAdoptionCenterRepository.Verify(x => x.GetComboAsync(), Times.Once);
        }

        [TestMethod]
        public async Task GetTotalPagesAsync_CallsRepositoryAndReturnsResult()
        {
            // Arrange
            var pagination = new PaginationDTO();
            var expectedResponse = new Response<int> { Result = 5 };
            _mockAdoptionCenterRepository.Setup(x => x.GetTotalPagesAsync(pagination)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.GetTotalPagesAsync(pagination);

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _mockAdoptionCenterRepository.Verify(x => x.GetTotalPagesAsync(pagination), Times.Once);
        }
    }
}
