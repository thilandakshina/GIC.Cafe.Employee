using Moq;
using NUnit.Framework;
using AutoMapper;
using Cafe.Application.DTOs;
using Cafe.Application.Queries.Cafe;
using Cafe.Application.Handlers.Cafe;
using Cafe.Application.Services;
using Cafe.Domain.Entities;
using Cafe.Domain.Models;

namespace Cafe.Application.Test
{
    [TestFixture]
    public class GetCafeByIdQueryHandlerTests
    {
        private Mock<ICafeService> _mockCafeService;
        private Mock<IMapper> _mockMapper;
        private GetCafeByIdQueryHandler _handler;

        [SetUp]
        public void Setup()
        {
            _mockCafeService = new Mock<ICafeService>();
            _mockMapper = new Mock<IMapper>();
            _handler = new GetCafeByIdQueryHandler(_mockCafeService.Object, _mockMapper.Object);
        }

        [Test]
        public async Task Handle_ValidId_ReturnsCafeDto()
        {
            // Arrange
            var cafeId = Guid.NewGuid();
            var query = new GetCafeByIdQuery { Id = cafeId };

            var cafe = new CafeEntity
            {
                Id = cafeId,
                Name = "Test Cafe"
            };

            var empList = new List<CafeEmployeeEntity>
            {
                new CafeEmployeeEntity(),
                new CafeEmployeeEntity()
            };

            var cafeWithEmployees = new CafeWithEmployees(cafe, empList);

            var expectedDto = new CafeDto
            {
                Id = cafeId,
                Name = "Test Cafe",
                EmployeeCount = 2
            };

            _mockCafeService.Setup(s => s.GetByIdAsync(cafeId))
                .ReturnsAsync(cafeWithEmployees);

            _mockMapper.Setup(m => m.Map<CafeDto>(cafe))
                .Returns(expectedDto);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(cafeId));
            Assert.That(result.EmployeeCount, Is.EqualTo(2));
            _mockCafeService.Verify(s => s.GetByIdAsync(cafeId), Times.Once);
        }

        [Test]
        public void Handle_ServiceThrowsException_ThrowsException()
        {
            // Arrange
            var cafeId = Guid.NewGuid();
            var query = new GetCafeByIdQuery { Id = cafeId };

            _mockCafeService.Setup(s => s.GetByIdAsync(cafeId))
                .ThrowsAsync(new ArgumentException($"Cafe with ID {cafeId} not found"));

            // Act & Assert
            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _handler.Handle(query, CancellationToken.None));

            Assert.That(exception.Message, Is.EqualTo($"Cafe with ID {cafeId} not found"));
        }
    }
}