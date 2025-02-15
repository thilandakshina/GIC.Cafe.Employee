using Moq;
using NUnit.Framework;
using Cafe.Application.Commands.Cafe;
using Cafe.Application.Handlers.Cafe;
using Cafe.Application.Services;

namespace Cafe.Application.Test
{
    [TestFixture]
    public class DeleteCafeCommandHandlerTests
    {
        private Mock<ICafeService> _mockCafeService;
        private DeleteCafeCommandHandler _handler;

        [SetUp]
        public void Setup()
        {
            _mockCafeService = new Mock<ICafeService>();
            _handler = new DeleteCafeCommandHandler(_mockCafeService.Object);
        }

        [Test]
        public async Task Handle_ValidCommand_ReturnsTrue()
        {
            // Arrange
            var cafeId = Guid.NewGuid();
            var command = new DeleteCafeCommand { Id = cafeId };

            _mockCafeService.Setup(s => s.DeleteAsync(cafeId))
                .ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result, Is.True);
            _mockCafeService.Verify(s => s.DeleteAsync(cafeId), Times.Once);
        }

        [Test]
        public async Task Handle_WhenServiceThrowsException_ThrowsException()
        {
            // Arrange
            var cafeId = Guid.NewGuid();
            var command = new DeleteCafeCommand { Id = cafeId };

            _mockCafeService.Setup(s => s.DeleteAsync(cafeId))
                .ThrowsAsync(new ArgumentException($"Cafe with ID {cafeId} not found"));

            // Act & Assert
            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _handler.Handle(command, CancellationToken.None));

            Assert.That(exception.Message, Is.EqualTo($"Cafe with ID {cafeId} not found"));
            _mockCafeService.Verify(s => s.DeleteAsync(cafeId), Times.Once);
        }

        [Test]
        public async Task Handle_WhenServiceReturnsFalse_ReturnsFalse()
        {
            // Arrange
            var cafeId = Guid.NewGuid();
            var command = new DeleteCafeCommand { Id = cafeId };

            _mockCafeService.Setup(s => s.DeleteAsync(cafeId))
                .ReturnsAsync(false);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result, Is.False);
            _mockCafeService.Verify(s => s.DeleteAsync(cafeId), Times.Once);
        }
    }
}