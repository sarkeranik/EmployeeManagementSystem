namespace EmployeeManagement.Domain.Projects.Features;

using EmployeeManagement.Domain.Projects.Services;
using EmployeeManagement.Domain.Projects;
using EmployeeManagement.Domain.Projects.Dtos;
using EmployeeManagement.Services;
using SharedKernel.Exceptions;
using MapsterMapper;
using MediatR;

public static class AddProject
{
    public sealed class Command : IRequest<ProjectDto>
    {
        public readonly ProjectForCreationDto ProjectToAdd;

        public Command(ProjectForCreationDto projectToAdd)
        {
            ProjectToAdd = projectToAdd;
        }
    }

    public sealed class Handler : IRequestHandler<Command, ProjectDto>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public Handler(IProjectRepository projectRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _projectRepository = projectRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ProjectDto> Handle(Command request, CancellationToken cancellationToken)
        {
            var project = Project.Create(request.ProjectToAdd);
            await _projectRepository.Add(project, cancellationToken);

            await _unitOfWork.CommitChanges(cancellationToken);

            var projectAdded = await _projectRepository.GetById(project.Id, cancellationToken: cancellationToken);
            return _mapper.Map<ProjectDto>(projectAdded);
        }
    }
}