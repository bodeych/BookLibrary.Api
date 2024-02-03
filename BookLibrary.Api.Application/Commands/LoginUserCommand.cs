using BookLibrary.Api.Application.Dto;
using BookLibrary.Api.Application.Interfaces;

namespace BookLibrary.Api.Application.Commands;

using BCrypt.Net;
public class LoginUserCommand : ICommand<TokensDto?>
{
    public required string Username { get; init; }
    public required string Password { get; init; }
}

internal sealed class LoginUserCommandHandler : ICommandHandler<LoginUserCommand, TokensDto?>
{
    private readonly IUserRepository _userRepository;
    
    public LoginUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<TokensDto?> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindByUsernameAsync(request.Username, cancellationToken);

        if (user == null || BCrypt.Verify(request.Password, user.Password) == false)
        {
            return null;
        }

        var tokensDto = new TokensDto
        {
            AccessToken = user.AccessToken,
            RefreshToken = user.RefreshToken
        };

        return tokensDto;
    }
}