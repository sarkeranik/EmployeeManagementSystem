namespace EmployeeManagement.Domain.Projects.Features;

using EmployeeManagement.Domain.Projects.Dtos;
using EmployeeManagement.Domain.Projects.Services;
using EmployeeManagement.Wrappers;
using SharedKernel.Exceptions;
using Microsoft.EntityFrameworkCore;
using MapsterMapper;
using Mapster;
using MediatR;
using Sieve.Models;
using Sieve.Services;

public static class GetProjectList
{
    public sealed class Query : IRequest<PagedList<ProjectDto>>
    {
        public readonly ProjectParametersDto QueryParameters;

        public Query(ProjectParametersDto queryParameters)
        {
            QueryParameters = queryParameters;
        }
    }

    public sealed class Handler : IRequestHandler<Query, PagedList<ProjectDto>>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly SieveProcessor _sieveProcessor;
        private readonly IMapper _mapper;

        public Handler(IProjectRepository projectRepository, IMapper mapper, SieveProcessor sieveProcessor)
        {
            _mapper = mapper;
            _projectRepository = projectRepository;
            _sieveProcessor = sieveProcessor;
        }

        public async Task<PagedList<ProjectDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var collection = _projectRepository.Query().AsNoTracking();

            var sieveModel = new SieveModel
            {
                Sorts = request.QueryParameters.SortOrder ?? "-CreatedOn",
                Filters = request.QueryParameters.Filters
            };

            var appliedCollection = _sieveProcessor.Apply(sieveModel, collection);
            var dtoCollection = appliedCollection
                .ProjectToType<ProjectDto>();

            return await PagedList<ProjectDto>.CreateAsync(dtoCollection,
                request.QueryParameters.PageNumber,
                request.QueryParameters.PageSize,
                cancellationToken);
        }
    }
}