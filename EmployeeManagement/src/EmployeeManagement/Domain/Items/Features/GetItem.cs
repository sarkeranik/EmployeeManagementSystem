namespace EmployeeManagement.Domain.Items.Features;

using EmployeeManagement.Domain.Items.Dtos;
using EmployeeManagement.Domain.Items.Services;
using SharedKernel.Exceptions;
using MapsterMapper;
using MediatR;

public static class GetItem
{
    public sealed class Query : IRequest<ItemDto>
    {
        public readonly Guid Id;

        public Query(Guid id)
        {
            Id = id;
        }
    }

    public sealed class Handler : IRequestHandler<Query, ItemDto>
    {
        private readonly IItemRepository _itemRepository;
        private readonly IMapper _mapper;

        public Handler(IItemRepository itemRepository, IMapper mapper)
        {
            _mapper = mapper;
            _itemRepository = itemRepository;
        }

        public async Task<ItemDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var result = await _itemRepository.GetById(request.Id, cancellationToken: cancellationToken);
            return _mapper.Map<ItemDto>(result);
        }
    }
}