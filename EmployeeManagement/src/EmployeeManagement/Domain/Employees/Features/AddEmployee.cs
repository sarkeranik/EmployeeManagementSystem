namespace EmployeeManagement.Domain.Employees.Features;

using EmployeeManagement.Domain.Employees.Services;
using EmployeeManagement.Domain.Employees;
using EmployeeManagement.Domain.Employees.Dtos;
using EmployeeManagement.Services;
using SharedKernel.Exceptions;
using MapsterMapper;
using MediatR;

public static class AddEmployee
{
    public sealed class Command : IRequest<EmployeeDto>
    {
        public readonly EmployeeForCreationDto EmployeeToAdd;

        public Command(EmployeeForCreationDto employeeToAdd)
        {
            EmployeeToAdd = employeeToAdd;
        }
    }

    public sealed class Handler : IRequestHandler<Command, EmployeeDto>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public Handler(IEmployeeRepository employeeRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _employeeRepository = employeeRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<EmployeeDto> Handle(Command request, CancellationToken cancellationToken)
        {
            var employee = Employee.Create(request.EmployeeToAdd);
            await _employeeRepository.Add(employee, cancellationToken);

            await _unitOfWork.CommitChanges(cancellationToken);

            var employeeAdded = await _employeeRepository.GetById(employee.Id, cancellationToken: cancellationToken);
            return _mapper.Map<EmployeeDto>(employeeAdded);
        }
    }
}