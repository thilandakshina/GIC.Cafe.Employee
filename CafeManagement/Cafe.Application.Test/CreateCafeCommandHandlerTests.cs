using Moq;
using AutoMapper;
using Cafe.Application.Handlers.Cafe;
using Cafe.Application.Services;
using Cafe.Application.Commands.Cafe;
using Cafe.Domain.Entities;

namespace Cafe.Application.Test
{
    [TestFixture]
    public class CreateCafeCommandHandlerTests
    {
        private Mock<ICafeService> _mockService;
        private Mock<IMapper> _mockMapper;
        private CreateCafeCommandHandler _handler;

        [SetUp]
        public void Setup()
        {
            _mockService = new Mock<ICafeService>();
            _mockMapper = new Mock<IMapper>();
            _handler = new CreateCafeCommandHandler(_mockService.Object, _mockMapper.Object);
        }

        [Test]
        public async Task Handle_ValidCommand_ReturnsGuid()
        {
            var command = new CreateCafeCommand
            {
                Name = "Cafe Test",
                Description = "Test Description",
                Location = "Test Location",
                Logo = "test-logo.png"
            };

            var cafeEntity = new CafeEntity
            {
                Id = Guid.NewGuid(),
                Name = command.Name,
                Description = command.Description,
                Location = command.Location,
                Logo = command.Logo
            };

            _mockMapper.Setup(m => m.Map<CafeEntity>(command))
                .Returns(cafeEntity);

            _mockService.Setup(s => s.CreateAsync(It.IsAny<CafeEntity>()))
                .ReturnsAsync(cafeEntity.Id);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.That(result, Is.EqualTo(cafeEntity.Id));
            _mockService.Verify(s => s.CreateAsync(It.IsAny<CafeEntity>()), Times.Once);
        }
    }
}