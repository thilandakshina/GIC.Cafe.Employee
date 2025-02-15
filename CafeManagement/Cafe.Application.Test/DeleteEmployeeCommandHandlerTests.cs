using Moq;
using Cafe.Application.Handlers.Employee;
using Cafe.Application.Services;
using Cafe.Application.Commands.Employee;

namespace Cafe.Application.Test
{
    [TestFixture]
    public class DeleteEmployeeCommandHandlerTests
    {
        private Mock<IEmployeeService> _mockEmployeeService;
        private DeleteEmployeeCommandHandler _handler;

        [SetUp]
        public void Setup()
        {
            _mockEmployeeService = new Mock<IEmployeeService>();
            _handler = new DeleteEmployeeCommandHandler(_mockEmployeeService.Object);
        }

        [Test]
        public async Task Handle_WhenEmployeeExists_ShouldReturnTrue()
        {
            var employeeId = Guid.NewGuid();
            var command = new DeleteEmployeeCommand { Id = employeeId };

            _mockEmployeeService.Setup(s => s.DeleteAsync(employeeId))
                .ReturnsAsync(true);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.That(result, Is.True);
            _mockEmployeeService.Verify(s => s.DeleteAsync(employeeId), Times.Once);
        }

        [Test]
        public async Task Handle_WhenEmployeeNotFound_ShouldThrowException()
        {
            var employeeId = Guid.NewGuid();
            var command = new DeleteEmployeeCommand { Id = employeeId };

            _mockEmployeeService.Setup(s => s.DeleteAsync(employeeId))
                .ThrowsAsync(new ArgumentException($"Employee with ID {employeeId} not found"));

            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _handler.Handle(command, CancellationToken.None));

            Assert.That(exception.Message, Is.EqualTo($"Employee with ID {employeeId} not found"));
        }
    }
}