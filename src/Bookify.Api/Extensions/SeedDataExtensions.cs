using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using Bookify.Application.Abstractions.Data;
using Bookify.Domain.Apartments;
using Dapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Bookify.Api.Extensions;

public static class SeedDataExtensions
{
    public static void SeedData(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        var sqlConnectionFactory = scope.ServiceProvider.GetRequiredService<ISqlConnectionFactory>();

        try
        {
            using var connection = sqlConnectionFactory.CreateConnection();
            if (connection is not NpgsqlConnection npgsqlConnection)
            {
                throw new InvalidOperationException("The connection is not a NpgsqlConnection");
            }

            npgsqlConnection.Open();

            var count = npgsqlConnection.ExecuteScalar<int>("SELECT COUNT(*) FROM apartments");
            if (count > 0)
            {
                logger.LogInformation("Data already seeded. Skipping.");
                return;
            }

            var faker = new Faker();
            var apartments = Enumerable.Range(0, 100).Select(_ => new
            {
                Id = Guid.NewGuid(),
                Name = faker.Company.CompanyName(),
                Description = "Amazing view",
                AddressCountry = faker.Address.Country(),
                AddressState = faker.Address.State(),
                AddressZipCode = faker.Address.ZipCode(),
                AddressCity = faker.Address.City(),
                AddressStreet = faker.Address.StreetAddress(),
                PriceAmount = faker.Random.Decimal(50, 1000),
                PriceCurrency = "USD",
                CleaningFeeAmount = faker.Random.Decimal(25, 200),
                CleaningFeeCurrency = "USD",
                Amenities = new int[] { (int)Amenity.Parking, (int)Amenity.MountainView },
                LastBookedOnUtc = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            }).ToList();

            const string sql = @"
                INSERT INTO apartments
                (id, name, description, address_country, address_state, address_zip_code, address_city, address_street, price_amount, price_currency, cleaning_fee_amount, cleaning_fee_currency, amenities, last_booked_on_utc)
                VALUES(@Id, @Name, @Description, @AddressCountry, @AddressState, @AddressZipCode, @AddressCity, @AddressStreet, @PriceAmount, @PriceCurrency, @CleaningFeeAmount, @CleaningFeeCurrency, @Amenities, @LastBookedOnUtc);
            ";

            npgsqlConnection.Execute(sql, apartments);

            logger.LogInformation("Seed data inserted successfully. {Count} apartments added.", apartments.Count);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding the database.");
        }
    }
}