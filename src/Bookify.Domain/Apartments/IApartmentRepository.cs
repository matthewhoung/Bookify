namespace Bookify.Domain.Apartments;

public interface IApartmentRepository
{
    Task<Apartment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task Add(Apartment apartment, CancellationToken cancellationToken = default);
}