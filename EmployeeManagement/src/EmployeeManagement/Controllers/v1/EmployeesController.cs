namespace EmployeeManagement.Controllers.v1;

using EmployeeManagement.Domain.Employees.Features;
using EmployeeManagement.Domain.Employees.Dtos;
using EmployeeManagement.Wrappers;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;
using System.Threading;
using MediatR;

[ApiController]
[Route("api/employees")]
[ApiVersion("1.0")]
public sealed class EmployeesController: ControllerBase
{
    private readonly IMediator _mediator;

    public EmployeesController(IMediator mediator)
    {
        _mediator = mediator;
    }
    

    /// <summary>
    /// Gets a list of all Employees.
    /// </summary>
    [HttpGet(Name = "GetEmployees")]
    public async Task<IActionResult> GetEmployees([FromQuery] EmployeeParametersDto employeeParametersDto)
    {
        var query = new GetEmployeeList.Query(employeeParametersDto);
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
    /// Gets a single Employee by ID.
    /// </summary>
    [HttpGet("{id:guid}", Name = "GetEmployee")]
    public async Task<ActionResult<EmployeeDto>> GetEmployee(Guid id)
    {
        var query = new GetEmployee.Query(id);
        var queryResponse = await _mediator.Send(query);

        return Ok(queryResponse);
    }


    /// <summary>
    /// Creates a new Employee record.
    /// </summary>
    [HttpPost(Name = "AddEmployee")]
    public async Task<ActionResult<EmployeeDto>> AddEmployee([FromBody]EmployeeForCreationDto employeeForCreation)
    {
        var command = new AddEmployee.Command(employeeForCreation);
        var commandResponse = await _mediator.Send(command);

        return CreatedAtRoute("GetEmployee",
            new { commandResponse.Id },
            commandResponse);
    }


    /// <summary>
    /// Updates an entire existing Employee.
    /// </summary>
    [HttpPut("{id:guid}", Name = "UpdateEmployee")]
    public async Task<IActionResult> UpdateEmployee(Guid id, EmployeeForUpdateDto employee)
    {
        var command = new UpdateEmployee.Command(id, employee);
        await _mediator.Send(command);

        return NoContent();
    }


    /// <summary>
    /// Deletes an existing Employee record.
    /// </summary>
    [HttpDelete("{id:guid}", Name = "DeleteEmployee")]
    public async Task<ActionResult> DeleteEmployee(Guid id)
    {
        var command = new DeleteEmployee.Command(id);
        await _mediator.Send(command);

        return NoContent();
    }

    // endpoint marker - do not delete this comment
}
