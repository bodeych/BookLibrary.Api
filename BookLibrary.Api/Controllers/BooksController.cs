using BookLibrary.Api.Application.Queries;
using BookLibrary.Api.Controllers.Contracts.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BookLibrary.Api.Controllers;

[ApiController]
[Route("api/v1/books")]
public class BooksController : ControllerBase
{
    private readonly IMediator _mediator;

    public BooksController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {

        var listOrdersQuery = new GetListBooksQuery();
        var books = await _mediator.Send(listOrdersQuery, cancellationToken);

        var orderDetailsResponse = books.Select(book => new BookDetailsResponse
        {
            Id = book.Id,
            UserId = book.UserId,
            Title = book.Title,
            Author = book.Author,
            Genre = book.Genre,
            PublicationYear = book.PublicationYear
        }).ToList();
        return Ok(orderDetailsResponse);
    }
}