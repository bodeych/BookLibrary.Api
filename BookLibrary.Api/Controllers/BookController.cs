using BookLibrary.Api.Application.Commands;
using BookLibrary.Api.Application.Queries;
using BookLibrary.Api.Controllers.Contracts.Requests;
using BookLibrary.Api.Controllers.Contracts.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BookLibrary.Api.Controllers;

[ApiController]
[Route("api/v1/book")]
public class BookController : ControllerBase
{
    private readonly IMediator _mediator;

    public BookController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateBook([FromBody] CreateBookRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var bookCommand = new CreateBookCommand
            {
                UserId = Guid.NewGuid(),
                Title = request.Title,
                Author = request.Author,
                Genre = request.Genre,
                PublicationYear = request.PublicationYear
            };

            var createdOrderId = await _mediator.Send(bookCommand, cancellationToken);

            var orderResponse = new CreateBookResponse
            {
                Id = createdOrderId
            };

            return Ok(orderResponse);
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("{bookId:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid bookId, CancellationToken cancellationToken)
    {
        var idBookDetailsQuery = new GetBookDetailsQuery
        {
            Id = bookId
        };

        var book = await _mediator.Send(idBookDetailsQuery, cancellationToken);

        if (book is null)
        {
            return NotFound();
        }

        var bookDetailsResponse = new BookDetailsResponse
        {
            Id = book.Id,
            UserId = book.UserId,
            Title = book.Title,
            Author = book.Author,
            Genre = book.Genre,
            PublicationYear = book.PublicationYear
        };

        return Ok(bookDetailsResponse);
    }
    
    [HttpDelete("{orderId:guid}")]
    public async Task<IActionResult> DeleteById([FromRoute] Guid orderId, CancellationToken cancellationToken)
    {

        var deleteBookCommand = new DeleteBookCommand
        {
            Id = orderId,
        };
        
        var order = await _mediator.Send(deleteBookCommand, cancellationToken);

        if (order is false)
        {
            return NotFound();
        }

        return Ok();
    }
}