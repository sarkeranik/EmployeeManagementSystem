namespace EmployeeManagement.Domain.UserProfiles.Dtos;

using SharedKernel.Dtos;

public sealed class UserProfileParametersDto : BasePaginationParameters
{
    public string Filters { get; set; }
    public string SortOrder { get; set; }
}
