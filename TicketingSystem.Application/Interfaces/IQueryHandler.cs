namespace TicketingSystem.Application.Interfaces;

public interface IQueryHandler<in TQuery, TResponse> 
    where TQuery : IQuery<TResponse>
{
    Task<TResponse> HandleAsync(TQuery query);
}