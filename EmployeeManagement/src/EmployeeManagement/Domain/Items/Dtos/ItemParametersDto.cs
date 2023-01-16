namespace EmployeeManagement.Domain.Items.Dtos;

using SharedKernel.Dtos;

public sealed class ItemParametersDto : BasePaginationParameters
{
    public string Filters { get; set; }
    public string SortOrder { get; set; }
}
