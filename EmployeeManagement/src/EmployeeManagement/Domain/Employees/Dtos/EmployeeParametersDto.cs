namespace EmployeeManagement.Domain.Employees.Dtos;

using SharedKernel.Dtos;

public sealed class EmployeeParametersDto : BasePaginationParameters
{
    public string Filters { get; set; }
    public string SortOrder { get; set; }
}
