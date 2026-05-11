using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using TicketingSystem.Application.Interfaces;
using TicketingSystem.Application.UseCases.Commands;
using TicketingSystem.Application.UseCases.Handlers;
using TicketingSystem.Domain.Entities;
using Xunit;

namespace TicketingSystem.Tests.UseCases
{
    public class ConfirmPaymentHandlerTests
    {
        private readonly Mock<IReservationRepository> _reservationRepoMock;
        private readonly Mock<ISeatRepository> _seatRepoMock;
        private readonly Mock<IAuditLogRepository> _auditRepoMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly ConfirmPaymentHandler _handler;

        public ConfirmPaymentHandlerTests()
        {
            // 1. Configuramos los Mocks
            _reservationRepoMock = new Mock<IReservationRepository>();
            _seatRepoMock = new Mock<ISeatRepository>();
            _auditRepoMock = new Mock<IAuditLogRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            // 2. Instanciamos el handler con los mocks
            _handler = new ConfirmPaymentHandler(
                _reservationRepoMock.Object,
                _seatRepoMock.Object,
                _auditRepoMock.Object,
                _unitOfWorkMock.Object
            );
        }

        [Fact]
        public async Task HandleAsync_ReservaInvalidaOPendiente_LanzaExcepcion()
        {
            // Arrange
            var command = new ConfirmPaymentCommand(Guid.NewGuid(), 1);
            
            // Simulamos que el repositorio no encuentra la reserva
            _reservationRepoMock.Setup(r => r.GetByIdAsync(command.ReservationId))
                .ReturnsAsync((Reservation?)null);

            // Act
            Func<Task> act = async () => await _handler.HandleAsync(command);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("La reserva es inválida, no te pertenece o ya no está pendiente.");
            
            // Verificamos que se abrió la transacción, pero nunca se confirmó
            _unitOfWorkMock.Verify(u => u.BeginTransactionAsync(), Times.Once);
            _unitOfWorkMock.Verify(u => u.CommitTransactionAsync(), Times.Never);
        }

        [Fact]
        public async Task HandleAsync_PagoExitoso_ActualizaEstadosYComitea()
        {
            // Arrange
            var userId = 100;
            var seatId = Guid.NewGuid();
            var reservationId = Guid.NewGuid();
            var command = new ConfirmPaymentCommand(reservationId, userId);

            var reservation = new Reservation(reservationId, userId, seatId, DateTime.UtcNow, DateTime.UtcNow.AddMinutes(5));
            
            var sector = new Sector("General", 1);
            var seat = new Seat(10, 1);
            typeof(Seat).GetProperty("Id")?.SetValue(seat, seatId);
            typeof(Seat).GetProperty("Sector")?.SetValue(seat, sector);
            seat.Reserve(); // Simulamos que estaba reservada previamente

            _reservationRepoMock.Setup(r => r.GetByIdAsync(reservationId)).ReturnsAsync(reservation);
            _seatRepoMock.Setup(s => s.GetByIdAsync(seatId)).ReturnsAsync(seat);

            // Act
            var result = await _handler.HandleAsync(command);

            // Assert
            result.Should().BeTrue();
            reservation.Status.Should().Be(ReservationStatus.Paid);
            seat.Status.Should().Be(SeatStatus.Sold);

            _reservationRepoMock.Verify(r => r.UpdateAsync(reservation), Times.Once);
            _seatRepoMock.Verify(s => s.UpdateAsync(seat), Times.Once);
            _auditRepoMock.Verify(a => a.AddAsync(It.Is<AuditLog>(log => log.Action == "PAYMENT_SUCCESS")), Times.Once);
            _unitOfWorkMock.Verify(u => u.CommitTransactionAsync(), Times.Once);
        }
    }
}