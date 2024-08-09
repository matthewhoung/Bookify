namespace Bookify.Api.Controllers.Users;

public sealed record class RegisterUserRequest(
    string Email,
    string FirstName,
    string LastName,
    string Password);
