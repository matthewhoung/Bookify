using Bookify.Application.Abstractions.Messaging;

namespace Bookify.Application.Users.LogInUser;

public sealed record class LogInUserCommand(string Email, string Password)
    : ICommand<AccessTokenResponse>;