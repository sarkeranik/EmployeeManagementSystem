namespace EmployeeManagement.Controllers.v1;

using EmployeeManagement.Domain.Clients.Features;
using EmployeeManagement.Domain.Clients.Dtos;
using EmployeeManagement.Wrappers;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;
using System.Threading;
using MediatR;

[ApiController]
[Route("api/clients")]
[ApiVersion("1.0")]
public sealed class ClientsController: ControllerBase
{
    private readonly IMediator _mediator;

    public ClientsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    

    /// <summary>
    /// Gets a list of all Clients.
    /// </summary>
    [HttpGet(Name = "GetClients")]
    public async Task<IActionResult> GetClients([FromQuery] ClientParametersDto clientParametersDto)
    {
        var query = new GetClientList.Query(clientParametersDto);
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
    /// Gets a single Client by ID.
    /// </summary>
    [HttpGet("{id:guid}", Name = "GetClient")]
    public async Task<ActionResult<ClientDto>> GetClient(Guid id)
    {
        var query = new GetClient.Query(id);
        var queryResponse = await _mediator.Send(query);

        return Ok(queryResponse);
    }


    /// <summary>
    /// Creates a new Client record.
    /// </summary>
    [HttpPost(Name = "AddClient")]
    public async Task<ActionResult<ClientDto>> AddClient([FromBody]ClientForCreationDto clientForCreation)
    {
        var command = new AddClient.Command(clientForCreation);
        var commandResponse = await _mediator.Send(command);

        return CreatedAtRoute("GetClient",
            new { commandResponse.Id },
            commandResponse);
    }


    /// <summary>
    /// Updates an entire existing Client.
    /// </summary>
    [HttpPut("{id:guid}", Name = "UpdateClient")]
    public async Task<IActionResult> UpdateClient(Guid id, ClientForUpdateDto client)
    {
        var command = new UpdateClient.Command(id, client);
        await _mediator.Send(command);

        return NoContent();
    }


    /// <summary>
    /// Deletes an existing Client record.
    /// </summary>
    [HttpDelete("{id:guid}", Name = "DeleteClient")]
    public async Task<ActionResult> DeleteClient(Guid id)
    {
        var command = new DeleteClient.Command(id);
        await _mediator.Send(command);

        return NoContent();
    }

    // endpoint marker - do not delete this comment
}
