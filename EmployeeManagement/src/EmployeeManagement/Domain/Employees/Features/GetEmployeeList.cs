namespace EmployeeManagement.Domain.Employees.Features;

using EmployeeManagement.Domain.Employees.Dtos;
using EmployeeManagement.Domain.Employees.Services;
using EmployeeManagement.Wrappers;
using SharedKernel.Exceptions;
using Microsoft.EntityFrameworkCore;
using MapsterMapper;
using Mapster;
using MediatR;
using Sieve.Models;
using Sieve.Services;

public static class GetEmployeeList
{
    public sealed class Query : IRequest<PagedList<EmployeeDto>>
    {
        public readonly EmployeeParametersDto QueryParameters;

        public Query(EmployeeParametersDto queryParameters)
        {
            QueryParameters = queryParameters;
        }
    }

    public sealed class Handler : IRequestHandler<Query, PagedList<EmployeeDto>>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly SieveProcessor _sieveProcessor;
        private readonly IMapper _mapper;

        public Handler(IEmployeeRepository employeeRepository, IMapper mapper, SieveProcessor sieveProcessor)
        {
            _mapper = mapper;
            _employeeRepository = employeeRepository;
            _sieveProcessor = sieveProcessor;
        }

        public async Task<PagedList<EmployeeDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var collection = _employeeRepository.Query().AsNoTracking();

            var sieveModel = new SieveModel
            {
                Sorts = request.QueryParameters.SortOrder ?? "-CreatedOn",
                Filters = request.QueryParameters.Filters
            };

            var appliedCollection = _sieveProcessor.Apply(sieveModel, collection);
            var dtoCollection = appliedCollection
                .ProjectToType<EmployeeDto>();

            return await PagedList<EmployeeDto>.CreateAsync(dtoCollection,
                request.QueryParameters.PageNumber,
                request.QueryParameters.PageSize,
                cancellationToken);
        }
    }
}