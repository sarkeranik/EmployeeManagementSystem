namespace EmployeeManagement.Domain.Clients.Features;

using EmployeeManagement.Domain.Clients.Dtos;
using EmployeeManagement.Domain.Clients.Services;
using EmployeeManagement.Wrappers;
using SharedKernel.Exceptions;
using Microsoft.EntityFrameworkCore;
using MapsterMapper;
using Mapster;
using MediatR;
using Sieve.Models;
using Sieve.Services;

public static class GetClientList
{
    public sealed class Query : IRequest<PagedList<ClientDto>>
    {
        public readonly ClientParametersDto QueryParameters;

        public Query(ClientParametersDto queryParameters)
        {
            QueryParameters = queryParameters;
        }
    }

    public sealed class Handler : IRequestHandler<Query, PagedList<ClientDto>>
    {
        private readonly IClientRepository _clientRepository;
        private readonly SieveProcessor _sieveProcessor;
        private readonly IMapper _mapper;

        public Handler(IClientRepository clientRepository, IMapper mapper, SieveProcessor sieveProcessor)
        {
            _mapper = mapper;
            _clientRepository = clientRepository;
            _sieveProcessor = sieveProcessor;
        }

        public async Task<PagedList<ClientDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var collection = _clientRepository.Query().AsNoTracking();

            var sieveModel = new SieveModel
            {
                Sorts = request.QueryParameters.SortOrder ?? "-CreatedOn",
                Filters = request.QueryParameters.Filters
            };

            var appliedCollection = _sieveProcessor.Apply(sieveModel, collection);
            var dtoCollection = appliedCollection
                .ProjectToType<ClientDto>();

            return await PagedList<ClientDto>.CreateAsync(dtoCollection,
                request.QueryParameters.PageNumber,
                request.QueryParameters.PageSize,
                cancellationToken);
        }
    }
}