namespace EmployeeManagement.Domain.Projects.Features;

using EmployeeManagement.Domain.Projects.Dtos;
using EmployeeManagement.Domain.Projects.Services;
using SharedKernel.Exceptions;
using MapsterMapper;
using MediatR;

public static class GetProject
{
    public sealed class Query : IRequest<ProjectDto>
    {
        public readonly Guid Id;

        public Query(Guid id)
        {
            Id = id;
        }
    }

    public sealed class Handler : IRequestHandler<Query, ProjectDto>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IMapper _mapper;

        public Handler(IProjectRepository projectRepository, IMapper mapper)
        {
            _mapper = mapper;
            _projectRepository = projectRepository;
        }

        public async Task<ProjectDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var result = await _projectRepository.GetById(request.Id, cancellationToken: cancellationToken);
            return _mapper.Map<ProjectDto>(result);
        }
    }
}