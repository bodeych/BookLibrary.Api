using BookLibrary.Api.Application.Dto;
using BookLibrary.Api.Application.Interfaces;
using BookLibrary.Api.Application.Service;

namespace BookLibrary.Api.Application.Commands;

public sealed class RefreshAccessTokenCommand : ICommand<TokensDto?>
{
    public required string AccessToken { get; init; }
    public required string RefreshToken { get; init; }
}

internal sealed class RefreshAccessTokenCommandHandler : ICommandHandler<RefreshAccessTokenCommand, TokensDto?>
{
    private readonly IUserRepository _userRepository;
    private readonly GenerateToken _generateToken;
    
    public RefreshAccessTokenCommandHandler(IUserRepository userRepository, GenerateToken generateToken)
    {
        _userRepository = userRepository;
        _generateToken = generateToken;
    }

    public async Task<TokensDto?> Handle(RefreshAccessTokenCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindByTokenAsync(request.RefreshToken, cancellationToken);
        if (user is null || user.RefreshToken != request.RefreshToken)
        {
            return null;
        }
         
        var id = user.Id;
         
        var newAccessToken = _generateToken.AccessToken(id);
        user.UpdateAccessToken(newAccessToken);
        await _userRepository.UpdateAsync(user, cancellationToken);
         
        var tokensDto = new TokensDto
        {
            AccessToken = user.AccessToken,
            RefreshToken = user.RefreshToken
        };

        return tokensDto;
    }
}