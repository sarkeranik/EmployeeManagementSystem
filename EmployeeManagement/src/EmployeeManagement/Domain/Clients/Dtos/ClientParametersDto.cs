namespace EmployeeManagement.Domain.Clients.Dtos;

using SharedKernel.Dtos;

public sealed class ClientParametersDto : BasePaginationParameters
{
    public string Filters { get; set; }
    public string SortOrder { get; set; }
}
