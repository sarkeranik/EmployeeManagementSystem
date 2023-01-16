namespace EmployeeManagement.Controllers.v1;

using EmployeeManagement.Domain.Projects.Features;
using EmployeeManagement.Domain.Projects.Dtos;
using EmployeeManagement.Wrappers;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;
using System.Threading;
using MediatR;

[ApiController]
[Route("api/projects")]
[ApiVersion("1.0")]
public sealed class ProjectsController: ControllerBase
{
    private readonly IMediator _mediator;

    public ProjectsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    

    /// <summary>
    /// Gets a list of all Projects.
    /// </summary>
    [HttpGet(Name = "GetProjects")]
    public async Task<IActionResult> GetProjects([FromQuery] ProjectParametersDto projectParametersDto)
    {
        var query = new GetProjectList.Query(projectParametersDto);
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
    /// Gets a single Project by ID.
    /// </summary>
    [HttpGet("{id:guid}", Name = "GetProject")]
    public async Task<ActionResult<ProjectDto>> GetProject(Guid id)
    {
        var query = new GetProject.Query(id);
        var queryResponse = await _mediator.Send(query);

        return Ok(queryResponse);
    }


    /// <summary>
    /// Creates a new Project record.
    /// </summary>
    [HttpPost(Name = "AddProject")]
    public async Task<ActionResult<ProjectDto>> AddProject([FromBody]ProjectForCreationDto projectForCreation)
    {
        var command = new AddProject.Command(projectForCreation);
        var commandResponse = await _mediator.Send(command);

        return CreatedAtRoute("GetProject",
            new { commandResponse.Id },
            commandResponse);
    }


    /// <summary>
    /// Updates an entire existing Project.
    /// </summary>
    [HttpPut("{id:guid}", Name = "UpdateProject")]
    public async Task<IActionResult> UpdateProject(Guid id, ProjectForUpdateDto project)
    {
        var command = new UpdateProject.Command(id, project);
        await _mediator.Send(command);

        return NoContent();
    }


    /// <summary>
    /// Deletes an existing Project record.
    /// </summary>
    [HttpDelete("{id:guid}", Name = "DeleteProject")]
    public async Task<ActionResult> DeleteProject(Guid id)
    {
        var command = new DeleteProject.Command(id);
        await _mediator.Send(command);

        return NoContent();
    }

    // endpoint marker - do not delete this comment
}
