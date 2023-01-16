namespace EmployeeManagement.Domain.Projects.Features;

using EmployeeManagement.Domain.Projects;
using EmployeeManagement.Domain.Projects.Dtos;
using EmployeeManagement.Domain.Projects.Services;
using EmployeeManagement.Services;
using SharedKernel.Exceptions;
using MapsterMapper;
using MediatR;

public static class UpdateProject
{
    public sealed class Command : IRequest<bool>
    {
        public readonly Guid Id;
        public readonly ProjectForUpdateDto UpdatedProjectData;

        public Command(Guid id, ProjectForUpdateDto updatedProjectData)
        {
            Id = id;
            UpdatedProjectData = updatedProjectData;
        }
    }

    public sealed class Handler : IRequestHandler<Command, bool>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IUnitOfWork _unitOfWork;

        public Handler(IProjectRepository projectRepository, IUnitOfWork unitOfWork)
        {
            _projectRepository = projectRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
        {
            var projectToUpdate = await _projectRepository.GetById(request.Id, cancellationToken: cancellationToken);

            projectToUpdate.Update(request.UpdatedProjectData);
            _projectRepository.Update(projectToUpdate);
            return await _unitOfWork.CommitChanges(cancellationToken) >= 1;
        }
    }
}