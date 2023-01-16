namespace EmployeeManagement.Domain.Projects.Dtos;

using SharedKernel.Dtos;

public sealed class ProjectParametersDto : BasePaginationParameters
{
    public string Filters { get; set; }
    public string SortOrder { get; set; }
}
