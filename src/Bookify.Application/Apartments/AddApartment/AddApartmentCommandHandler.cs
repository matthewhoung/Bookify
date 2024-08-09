using Bookify.Application.Abstractions.Messaging;
using Bookify.Domain.Abstractions;
using Bookify.Domain.Apartments;
using Bookify.Domain.Shared;

namespace Bookify.Application.Apartments.AddApartment;

internal sealed class AddApartmentCommandHandler : ICommandHandler<AddApartmentCommand, Guid>
{
    private readonly IApartmentRepository _apartmentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddApartmentCommandHandler(IApartmentRepository apartmentRepository, IUnitOfWork unitOfWork)
    {
        _apartmentRepository = apartmentRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(AddApartmentCommand command, CancellationToken cancellationToken)
    {
        var apartment = Apartment.Add(
            new Name(command.Name),
            new Description(command.Description),
            new Address(command.Country, command.State, command.ZipCode, command.City, command.Street),
            new Money(command.PriceAmount, Currency.FromCode(command.PriceCurrency)),
            new Money(command.CleaningFeeAmount, Currency.FromCode(command.CleaningFeeCurrency)),
            command.Amenities
        );

        _apartmentRepository.Add(apartment);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return apartment.Id;
    }
}
