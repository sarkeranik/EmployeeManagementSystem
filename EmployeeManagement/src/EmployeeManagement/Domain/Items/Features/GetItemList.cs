namespace EmployeeManagement.Domain.Items.Features;

using EmployeeManagement.Domain.Items.Dtos;
using EmployeeManagement.Domain.Items.Services;
using EmployeeManagement.Wrappers;
using SharedKernel.Exceptions;
using Microsoft.EntityFrameworkCore;
using MapsterMapper;
using Mapster;
using MediatR;
using Sieve.Models;
using Sieve.Services;

public static class GetItemList
{
    public sealed class Query : IRequest<PagedList<ItemDto>>
    {
        public readonly ItemParametersDto QueryParameters;

        public Query(ItemParametersDto queryParameters)
        {
            QueryParameters = queryParameters;
        }
    }

    public sealed class Handler : IRequestHandler<Query, PagedList<ItemDto>>
    {
        private readonly IItemRepository _itemRepository;
        private readonly SieveProcessor _sieveProcessor;
        private readonly IMapper _mapper;

        public Handler(IItemRepository itemRepository, IMapper mapper, SieveProcessor sieveProcessor)
        {
            _mapper = mapper;
            _itemRepository = itemRepository;
            _sieveProcessor = sieveProcessor;
        }

        public async Task<PagedList<ItemDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var collection = _itemRepository.Query().AsNoTracking();

            var sieveModel = new SieveModel
            {
                Sorts = request.QueryParameters.SortOrder ?? "-CreatedOn",
                Filters = request.QueryParameters.Filters
            };

            var appliedCollection = _sieveProcessor.Apply(sieveModel, collection);
            var dtoCollection = appliedCollection
                .ProjectToType<ItemDto>();

            return await PagedList<ItemDto>.CreateAsync(dtoCollection,
                request.QueryParameters.PageNumber,
                request.QueryParameters.PageSize,
                cancellationToken);
        }
    }
}