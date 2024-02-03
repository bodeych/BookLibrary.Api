using BookLibrary.Api.Application.Interfaces;
using BookLibrary.Api.Application.Service;
using BookLibrary.Api.Domain.Entities;

namespace BookLibrary.Api.Application.Commands;

using BCrypt.Net;
public sealed class RegistrationUserCommand : ICommand<bool>
{
    public required string Username { get; init; }
    public required string Password { get; init; }
}

internal sealed class RegistrationUserCommandHandler : ICommandHandler<RegistrationUserCommand, bool>
{
    private readonly IUserRepository _userRepository;
    private readonly GenerateToken _generateToken;
    
    public RegistrationUserCommandHandler(IUserRepository userRepository, GenerateToken generateToken)
    {
        _userRepository = userRepository;
        _generateToken = generateToken;
    }

    public async Task<bool> Handle(RegistrationUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _userRepository.FindByUsernameAsync(request.Username, cancellationToken);
        if (existingUser != null)
        {
            return false;
        }

        var id = Guid.NewGuid();
        var user = request.Username;
        var password = BCrypt.HashPassword(request.Password);
        var accessToken = _generateToken.AccessToken(id);
        var refreshToken = _generateToken.RefreshToken();
        var createdUser = User.Create(id, user, password, accessToken, refreshToken);

        await _userRepository.AddAsync(createdUser, cancellationToken);
        
        return true;
    }
}