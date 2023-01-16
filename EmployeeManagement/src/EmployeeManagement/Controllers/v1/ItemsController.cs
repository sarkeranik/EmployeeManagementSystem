namespace EmployeeManagement.Controllers.v1;

using EmployeeManagement.Domain.Items.Features;
using EmployeeManagement.Domain.Items.Dtos;
using EmployeeManagement.Wrappers;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;
using System.Threading;
using MediatR;

[ApiController]
[Route("api/items")]
[ApiVersion("1.0")]
public sealed class ItemsController: ControllerBase
{
    private readonly IMediator _mediator;

    public ItemsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    

    /// <summary>
    /// Gets a list of all Items.
    /// </summary>
    [HttpGet(Name = "GetItems")]
    public async Task<IActionResult> GetItems([FromQuery] ItemParametersDto itemParametersDto)
    {
        var query = new GetItemList.Query(itemParametersDto);
        var queryResponse = await _mediator.Send(query);

        var paginationMetadata = new
        {
            totalCount = queryResponse.TotalCount,
            pageSize = queryResponse.PageSize,
            currentPageSize = queryResponse.CurrentPageSize,
            currentStartIndex = queryResponse.CurrentStartIndex,
            currentEndIndex = queryResponse.CurrentEndIndex,
            pageNumber = queryResponse.PageNumber,
            totalPages = queryResponse.TotalPages,
            hasPrevious = queryResponse.HasPrevious,
            hasNext = queryResponse.HasNext
        };

        Response.Headers.Add("X-Pagination",
            JsonSerializer.Serialize(paginationMetadata));

        return Ok(queryResponse);
    }


    /// <summary>
    /// Gets a single Item by ID.
    /// </summary>
    [HttpGet("{id:guid}", Name = "GetItem")]
    public async Task<ActionResult<ItemDto>> GetItem(Guid id)
    {
        var query = new GetItem.Query(id);
        var queryResponse = await _mediator.Send(query);

        return Ok(queryResponse);
    }


    /// <summary>
    /// Creates a new Item record.
    /// </summary>
    [HttpPost(Name = "AddItem")]
    public async Task<ActionResult<ItemDto>> AddItem([FromBody]ItemForCreationDto itemForCreation)
    {
        var command = new AddItem.Command(itemForCreation);
        var commandResponse = await _mediator.Send(command);

        return CreatedAtRoute("GetItem",
            new { commandResponse.Id },
            commandResponse);
    }


    /// <summary>
    /// Updates an entire existing Item.
    /// </summary>
    [HttpPut("{id:guid}", Name = "UpdateItem")]
    public async Task<IActionResult> UpdateItem(Guid id, ItemForUpdateDto item)
    {
        var command = new UpdateItem.Command(id, item);
        await _mediator.Send(command);

        return NoContent();
    }


    /// <summary>
    /// Deletes an existing Item record.
    /// </summary>
    [HttpDelete("{id:guid}", Name = "DeleteItem")]
    public async Task<ActionResult> DeleteItem(Guid id)
    {
        var command = new DeleteItem.Command(id);
        await _mediator.Send(command);

        return NoContent();
    }

    // endpoint marker - do not delete this comment
}
