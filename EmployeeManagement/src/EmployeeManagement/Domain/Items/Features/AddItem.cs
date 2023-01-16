namespace EmployeeManagement.Domain.Items.Features;

using EmployeeManagement.Domain.Items.Services;
using EmployeeManagement.Domain.Items;
using EmployeeManagement.Domain.Items.Dtos;
using EmployeeManagement.Services;
using SharedKernel.Exceptions;
using MapsterMapper;
using MediatR;

public static class AddItem
{
    public sealed class Command : IRequest<ItemDto>
    {
        public readonly ItemForCreationDto ItemToAdd;

        public Command(ItemForCreationDto itemToAdd)
        {
            ItemToAdd = itemToAdd;
        }
    }

    public sealed class Handler : IRequestHandler<Command, ItemDto>
    {
        private readonly IItemRepository _itemRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public Handler(IItemRepository itemRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _itemRepository = itemRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ItemDto> Handle(Command request, CancellationToken cancellationToken)
        {
            var item = Item.Create(request.ItemToAdd);
            await _itemRepository.Add(item, cancellationToken);

            await _unitOfWork.CommitChanges(cancellationToken);

            var itemAdded = await _itemRepository.GetById(item.Id, cancellationToken: cancellationToken);
            return _mapper.Map<ItemDto>(itemAdded);
        }
    }
}