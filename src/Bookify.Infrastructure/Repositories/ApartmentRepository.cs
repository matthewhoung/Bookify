using Bookify.Domain.Apartments;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Infrastructure.Repositories;

internal sealed class ApartmentRepository : Repository<Apartment>, IApartmentRepository
{
    public ApartmentRepository(ApplicationDbContext dbContext)
        : base(dbContext)
    {
    }

    public async Task Add(Apartment apartment, CancellationToken cancellationToken = default)
    {
        await DbContext.Set<Apartment>().AddAsync(apartment, cancellationToken);
    }
}