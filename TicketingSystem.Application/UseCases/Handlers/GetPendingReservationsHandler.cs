using System.Collections.Generic;
using System.Threading.Tasks;
using TicketingSystem.Application.Interfaces;
using TicketingSystem.Application.UseCases.Queries;

namespace TicketingSystem.Application.UseCases.Handlers
{
    public class GetPendingReservationsHandler : IQueryHandler<GetPendingReservationsQuery, IEnumerable<PendingReservationDto>>
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly ISeatRepository _seatRepository;

        public GetPendingReservationsHandler(IReservationRepository reservationRepository, ISeatRepository seatRepository)
        {
            _reservationRepository = reservationRepository;
            _seatRepository = seatRepository;
        }

        public async Task<IEnumerable<PendingReservationDto>> HandleAsync(GetPendingReservationsQuery query)
        {
            var pendingReservations = await _reservationRepository.GetPendingByUserIdAsync(query.UserId);
            var result = new List<PendingReservationDto>();

            foreach (var reservation in pendingReservations)
            {
                var seat = await _seatRepository.GetByIdAsync(reservation.SeatId);
                if (seat != null && seat.Sector != null)
                {
                    result.Add(new PendingReservationDto(
                        reservation.Id, seat.Id, seat.Number, seat.Row, seat.Sector.Name, seat.Sector.Price, reservation.ExpiresAt
                    ));
                }
            }

            return result;
        }
    }
}