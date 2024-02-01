using BookLibrary.Api.Application.Commands;
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
}