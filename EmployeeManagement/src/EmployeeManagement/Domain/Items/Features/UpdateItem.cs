namespace EmployeeManagement.Domain.Items.Features;

using EmployeeManagement.Domain.Items;
using EmployeeManagement.Domain.Items.Dtos;
using EmployeeManagement.Domain.Items.Services;
using EmployeeManagement.Services;
using SharedKernel.Exceptions;
using MapsterMapper;
using MediatR;

public static class UpdateItem
{
    public sealed class Command : IRequest<bool>
    {
        public readonly Guid Id;
        public readonly ItemForUpdateDto UpdatedItemData;

        public Command(Guid id, ItemForUpdateDto updatedItemData)
        {
            Id = id;
            UpdatedItemData = updatedItemData;
        }
    }

    public sealed class Handler : IRequestHandler<Command, bool>
    {
        private readonly IItemRepository _itemRepository;
        private readonly IUnitOfWork _unitOfWork;

        public Handler(IItemRepository itemRepository, IUnitOfWork unitOfWork)
        {
            _itemRepository = itemRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
        {
            var itemToUpdate = await _itemRepository.GetById(request.Id, cancellationToken: cancellationToken);

            itemToUpdate.Update(request.UpdatedItemData);
            _itemRepository.Update(itemToUpdate);
            return await _unitOfWork.CommitChanges(cancellationToken) >= 1;
        }
    }
}