namespace EmployeeManagement.Domain.Employees.Features;

using EmployeeManagement.Domain.Employees.Dtos;
using EmployeeManagement.Domain.Employees.Services;
using SharedKernel.Exceptions;
using MapsterMapper;
using MediatR;

public static class GetEmployee
{
    public sealed class Query : IRequest<EmployeeDto>
    {
        public readonly Guid Id;

        public Query(Guid id)
        {
            Id = id;
        }
    }

    public sealed class Handler : IRequestHandler<Query, EmployeeDto>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;

        public Handler(IEmployeeRepository employeeRepository, IMapper mapper)
        {
            _mapper = mapper;
            _employeeRepository = employeeRepository;
        }

        public async Task<EmployeeDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var result = await _employeeRepository.GetById(request.Id, cancellationToken: cancellationToken);
            return _mapper.Map<EmployeeDto>(result);
        }
    }
}