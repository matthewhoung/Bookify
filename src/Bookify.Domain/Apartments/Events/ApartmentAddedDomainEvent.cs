using Bookify.Domain.Abstractions;

namespace Bookify.Domain.Apartments.Events;

public sealed record ApartmentAddedDomainEvent(Guid ApartmentId) : IDomainEvent;
