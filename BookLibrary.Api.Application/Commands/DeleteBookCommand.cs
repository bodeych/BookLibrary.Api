using BookLibrary.Api.Application.Interfaces;
using BookLibrary.Api.Domain.Entities;

namespace BookLibrary.Api.Application.Commands;

public class DeleteBookCommand: ICommand<bool>
{
    public required Guid Id { get; init; }
}

internal sealed class DeleteBookCommandHandler : ICommandHandler<DeleteBookCommand, bool>
{
    private readonly IBookRepository _bookRepository;

    public DeleteBookCommandHandler(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }
    public async Task<bool> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
    {
        var book = await _bookRepository.GetByIdAsync(request.Id, cancellationToken);
        
        if (book is null)
        {
            return false;
        }
        
        await _bookRepository.DeleteAsync(request.Id, cancellationToken);

        return true;
    }
}