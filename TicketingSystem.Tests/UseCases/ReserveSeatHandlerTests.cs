using System;
using System.Threading.Tasks;
using Moq;
using Xunit;
using FluentAssertions;
using TicketingSystem.Application.UseCases.Handlers;
using TicketingSystem.Application.UseCases.Commands;
using TicketingSystem.Application.Interfaces;
using TicketingSystem.Domain.Entities;

namespace TicketingSystem.Tests.UseCases
{
    public class ReserveSeatHandlerTests
    {
        private readonly Mock<ISeatRepository> _seatRepoMock;
        private readonly Mock<IReservationRepository> _reservationRepoMock;
        private readonly Mock<IAuditLogRepository> _auditRepoMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly ReserveSeatHandler _handler;

        public ReserveSeatHandlerTests()
        {
            // 1. Configuramos los Mocks (objetos simulados)
            _seatRepoMock = new Mock<ISeatRepository>();
            _reservationRepoMock = new Mock<IReservationRepository>();
            _auditRepoMock = new Mock<IAuditLogRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            // 2. Instanciamos el Handler pasándole los Mocks en lugar de repositorios reales
            _handler = new ReserveSeatHandler(
                _seatRepoMock.Object,
                _reservationRepoMock.Object,
                _auditRepoMock.Object,
                _unitOfWorkMock.Object
            );
        }

        [Fact]
        public async Task HandleAsync_SeatIsAlreadyReserved_ThrowsInvalidOperationException_And_Rollbacks()
        {
            // Arrange (Preparar)
            var eventId = 1;
            var seatId = Guid.NewGuid();
            var userId = 100;
            var command = new ReserveSeatCommand(eventId, seatId, userId);

            // Creamos una butaca simulada y la marcamos como "Reserved" (Reservada)
            var sectorMock = new Sector("VIP", eventId);
            var seatMock = new Seat(1, sectorMock.Id);
            
            // Usamos un truco con reflection para setear la propiedad privada Sector (solo para tests)
            typeof(Seat).GetProperty("Sector")?.SetValue(seatMock, sectorMock);
            
            seatMock.Reserve(); // Ya está reservada!

            // Le decimos al repositorio simulado que devuelva esta butaca ocupada
            _seatRepoMock.Setup(repo => repo.GetByIdAsync(seatId))
                         .ReturnsAsync(seatMock);

            // Act (Actuar)
            // Ejecutamos el comando y capturamos la excepción
            Func<Task> act = async () => await _handler.HandleAsync(command);

            // Assert (Verificar)
            // Verificamos que efectivamente lance el error indicando que no está disponible
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("La butaca no está disponible o no pertenece al evento especificado.");

            // Verificamos que se haya iniciado una transacción
            _unitOfWorkMock.Verify(uow => uow.BeginTransactionAsync(), Times.Once);

            // Verificamos que se haya hecho un ROLLBACK (porque hubo un error)
            _unitOfWorkMock.Verify(uow => uow.RollbackTransactionAsync(), Times.Once);

            // Verificamos que NO se haya guardado nada (Commit nunca debió llamarse)
            _unitOfWorkMock.Verify(uow => uow.CommitTransactionAsync(), Times.Never);
        }

        [Fact]
        public async Task HandleAsync_SeatIsAvailable_ReservesSeatAndCommitsSuccessfully()
        {
            // Arrange
            var eventId = 1;
            var seatId = Guid.NewGuid();
            var userId = 100;
            var command = new ReserveSeatCommand(eventId, seatId, userId);

            // Creamos una butaca simulada y la marcamos como "Available"
            var sectorMock = new Sector("VIP", eventId);
            var seatMock = new Seat(1, sectorMock.Id);
            typeof(Seat).GetProperty("Sector")?.SetValue(seatMock, sectorMock); // Truco de reflection

            // Le decimos al repositorio simulado que devuelva esta butaca disponible
            _seatRepoMock.Setup(repo => repo.GetByIdAsync(seatId))
                         .ReturnsAsync(seatMock);

            // Act
            var result = await _handler.HandleAsync(command);

            // Assert
            // 1. Verificamos que la reserva se creó correctamente
            result.Should().NotBeNull();
            result.SeatId.Should().Be(seatId);
            result.UserId.Should().Be(userId);
            result.Status.Should().Be(ReservationStatus.Pending);

            // 2. Verificamos que el estado de la butaca cambió en el dominio
            seatMock.Status.Should().Be(SeatStatus.Reserved);

            // 3. Verificamos que se llamó a los repositorios para guardar los datos
            _reservationRepoMock.Verify(repo => repo.AddAsync(It.IsAny<Reservation>()), Times.Once);
            _seatRepoMock.Verify(repo => repo.UpdateAsync(seatMock), Times.Once);
            _auditRepoMock.Verify(repo => repo.AddAsync(It.Is<AuditLog>(log => log.Action == "RESERVE_SUCCESS")), Times.Once);

            // 4. Verificamos que la transacción se completó (COMMIT)
            _unitOfWorkMock.Verify(uow => uow.BeginTransactionAsync(), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.CommitTransactionAsync(), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.RollbackTransactionAsync(), Times.Never); // No debe haber rollback
        }

        [Fact]
        public async Task HandleAsync_SeatDoesNotExist_ThrowsInvalidOperationException()
        {
            // Arrange
            var command = new ReserveSeatCommand(1, Guid.NewGuid(), 100);

            // Configuramos el mock para que devuelva null, simulando que no encontró la butaca
            _seatRepoMock.Setup(repo => repo.GetByIdAsync(command.SeatId))
                         .ReturnsAsync((Seat?)null);

            // Act
            Func<Task> act = async () => await _handler.HandleAsync(command);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("La butaca no está disponible o no pertenece al evento especificado.");
            _unitOfWorkMock.Verify(uow => uow.RollbackTransactionAsync(), Times.Once);
        }
    }
}