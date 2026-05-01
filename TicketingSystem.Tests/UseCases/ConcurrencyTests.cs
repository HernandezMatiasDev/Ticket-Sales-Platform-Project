using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using TicketingSystem.Application.UseCases.Commands;
using TicketingSystem.Application.UseCases.Handlers;
using TicketingSystem.Application.Interfaces;
using TicketingSystem.Domain.Entities;
using TicketingSystem.Infrastructure.Persistence;
using TicketingSystem.Infrastructure.Repositories;
using Xunit;

namespace TicketingSystem.Tests.Integration
{
    public class ConcurrencyTests : IDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly DbContextOptions<TicketingDbContext> _options;

        public ConcurrencyTests()
        {
            // SQLite en memoria SÍ soporta transacciones reales. La conexión debe mantenerse abierta.
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            _options = new DbContextOptionsBuilder<TicketingDbContext>()
                .UseSqlite(_connection)
                .Options;

            using var context = new TicketingDbContext(_options);
            context.Database.EnsureCreated(); // Crea el esquema de la base de datos
        }

        public void Dispose()
        {
            _connection.Dispose();
        }

        [Fact]
        public async Task ReserveSeat_WhenTwoUsersTryToReserveSameSeat_SecondOneFailsWithConcurrencyException()
        {
            var seatId = Guid.NewGuid();
            // Usamos Ids altos (99) para no entrar en conflicto con el Seeding de TicketingDbContext que ya usa el Id 1
            var eventId = 99;

            // 1. Seedeamos la base de datos con un asiento disponible
            using (var context = new TicketingDbContext(_options))
            {
                var sector = new Sector("VIP", eventId);
                typeof(Sector).GetProperty("Id")?.SetValue(sector, 99);

                var seat = new Seat(1, sector.Id);
                typeof(Seat).GetProperty("Id")?.SetValue(seat, seatId);

                // Asignamos el sector a la butaca para que la validación del handler funcione
                typeof(Seat).GetProperty("Sector")?.SetValue(seat, sector);

                context.Events.Add(new Event("Concierto", DateTime.UtcNow, "Estadio") { Id = eventId });
                context.Sectors.Add(sector);
                context.Seats.Add(seat);
                await context.SaveChangesAsync();
            }

            // 2. Simulamos dos usuarios (dos contextos de BD diferentes apuntando a la misma BD en memoria)
            var contextUser1 = new TicketingDbContext(_options);
            var contextUser2 = new TicketingDbContext(_options);

            // 3. AMBOS usuarios leen el asiento AL MISMO TIEMPO (antes de que se guarde ninguna reserva)
            var seatUser1 = await contextUser1.Seats.FirstOrDefaultAsync(s => s.Id == seatId);
            var seatUser2 = await contextUser2.Seats.FirstOrDefaultAsync(s => s.Id == seatId);

            // Act: Usuario 1 reserva y guarda (Gana la carrera)
            seatUser1!.Reserve();
            await contextUser1.SaveChangesAsync();

            // Act: Usuario 2 intenta reservar y guardar su copia desactualizada (Pierde la carrera)
            seatUser2!.Reserve();
            Func<Task> actUser2 = async () => await contextUser2.SaveChangesAsync();

            // Assert
            // La acción del Usuario 2 DEBE fallar con una DbUpdateConcurrencyException.
            // Esto prueba que tu API devolverá un 409 Conflict, como programaste en el Controller.
            await actUser2.Should().ThrowAsync<DbUpdateConcurrencyException>();
        }
    }
}