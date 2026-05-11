using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using TicketingSystem.Application.Interfaces;
using TicketingSystem.Domain.Entities;
using TicketingSystem.Infrastructure.Workers;
using Xunit;

namespace TicketingSystem.Tests.UseCases
{
    public class ExpiredReservationWorkerTests
    {
        private readonly Mock<IServiceProvider> _serviceProviderMock;
        private readonly Mock<IServiceScopeFactory> _serviceScopeFactoryMock;
        private readonly Mock<IServiceScope> _serviceScopeMock;
        private readonly Mock<ILogger<ExpiredReservationWorker>> _loggerMock;

        private readonly Mock<IReservationRepository> _reservationRepoMock;
        private readonly Mock<ISeatRepository> _seatRepoMock;
        private readonly Mock<IAuditLogRepository> _auditRepoMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;

        private readonly ExpiredReservationWorker _worker;

        public ExpiredReservationWorkerTests()
        {
            // Instanciamos los Mocks del contenedor de dependencias (necesarios para el Worker)
            _serviceProviderMock = new Mock<IServiceProvider>();
            _serviceScopeFactoryMock = new Mock<IServiceScopeFactory>();
            _serviceScopeMock = new Mock<IServiceScope>();
            _loggerMock = new Mock<ILogger<ExpiredReservationWorker>>();

            // Instanciamos los Mocks de los Repositorios
            _reservationRepoMock = new Mock<IReservationRepository>();
            _seatRepoMock = new Mock<ISeatRepository>();
            _auditRepoMock = new Mock<IAuditLogRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            // Setup de la fábrica de Scopes
            _serviceProviderMock.Setup(sp => sp.GetService(typeof(IServiceScopeFactory)))
                .Returns(_serviceScopeFactoryMock.Object);
            _serviceScopeFactoryMock.Setup(ssf => ssf.CreateScope())
                .Returns(_serviceScopeMock.Object);
            _serviceScopeMock.Setup(ss => ss.ServiceProvider)
                .Returns(_serviceProviderMock.Object);

            // Setup de las dependencias inyectadas dentro del Scope
            _serviceProviderMock.Setup(sp => sp.GetService(typeof(IReservationRepository))).Returns(_reservationRepoMock.Object);
            _serviceProviderMock.Setup(sp => sp.GetService(typeof(ISeatRepository))).Returns(_seatRepoMock.Object);
            _serviceProviderMock.Setup(sp => sp.GetService(typeof(IAuditLogRepository))).Returns(_auditRepoMock.Object);
            _serviceProviderMock.Setup(sp => sp.GetService(typeof(IUnitOfWork))).Returns(_unitOfWorkMock.Object);

            _worker = new ExpiredReservationWorker(_serviceProviderMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task ExecuteAsync_ConReservasExpiradas_LiberaButacasYComitea()
        {
            // Arrange
            var seatId = Guid.NewGuid();
            var reservation = new Reservation(Guid.NewGuid(), 1, seatId, DateTime.UtcNow.AddMinutes(-10), DateTime.UtcNow.AddMinutes(-5));
            var seat = new Seat(1, 1);
            typeof(Seat).GetProperty("Id")?.SetValue(seat, seatId);
            seat.Reserve(); // Status actual = Reserved

            var expiredReservations = new List<Reservation> { reservation };
            var cts = new CancellationTokenSource();

            // Cancelamos el token dentro de la llamada para asegurar que el while(!stoppingToken.IsCancellationRequested) se rompa
            _reservationRepoMock.Setup(r => r.GetExpiredPendingReservationsAsync(It.IsAny<DateTime>()))
                .ReturnsAsync(expiredReservations)
                .Callback(() => cts.Cancel());

            _seatRepoMock.Setup(s => s.GetByIdAsync(seatId)).ReturnsAsync(seat);

            // Usamos reflexión para acceder al método protegido
            var executeAsyncMethod = typeof(ExpiredReservationWorker).GetMethod("ExecuteAsync", BindingFlags.NonPublic | BindingFlags.Instance);
            
            // Act - Interceptamos el TaskCanceledException que salta por el Task.Delay(..., cancellationToken)
            var act = async () => await (Task)executeAsyncMethod!.Invoke(_worker, new object[] { cts.Token });
            await act.Should().ThrowAsync<TaskCanceledException>(); // Lanzado directamente por Task.Delay al cancelar el token

            // Assert
            reservation.Status.Should().Be(ReservationStatus.Expired);
            seat.Status.Should().Be(SeatStatus.Available);

            _reservationRepoMock.Verify(r => r.UpdateAsync(reservation), Times.Once);
            _seatRepoMock.Verify(s => s.UpdateAsync(seat), Times.Once);
            _auditRepoMock.Verify(a => a.AddAsync(It.Is<AuditLog>(log => log.Action == "RESERVE_EXPIRED")), Times.Once);
            _unitOfWorkMock.Verify(u => u.CommitTransactionAsync(), Times.Once);
        }
    }
}