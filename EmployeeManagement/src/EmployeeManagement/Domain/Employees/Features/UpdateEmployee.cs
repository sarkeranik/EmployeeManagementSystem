namespace EmployeeManagement.Domain.Employees.Features;

using EmployeeManagement.Domain.Employees;
using EmployeeManagement.Domain.Employees.Dtos;
using EmployeeManagement.Domain.Employees.Services;
using EmployeeManagement.Services;
using SharedKernel.Exceptions;
using MapsterMapper;
using MediatR;

public static class UpdateEmployee
{
    public sealed class Command : IRequest<bool>
    {
        public readonly Guid Id;
        public readonly EmployeeForUpdateDto UpdatedEmployeeData;

        public Command(Guid id, EmployeeForUpdateDto updatedEmployeeData)
        {
            Id = id;
            UpdatedEmployeeData = updatedEmployeeData;
        }
    }

    public sealed class Handler : IRequestHandler<Command, bool>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public Handler(IEmployeeRepository employeeRepository, IUnitOfWork unitOfWork)
        {
            _employeeRepository = employeeRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
        {
            var employeeToUpdate = await _employeeRepository.GetById(request.Id, cancellationToken: cancellationToken);

            employeeToUpdate.Update(request.UpdatedEmployeeData);
            _employeeRepository.Update(employeeToUpdate);
            return await _unitOfWork.CommitChanges(cancellationToken) >= 1;
        }
    }
}