namespace EmployeeManagement.Domain.Items.Features;

using EmployeeManagement.Domain.Items.Services;
using EmployeeManagement.Services;
using SharedKernel.Exceptions;
using MediatR;

public static class DeleteItem
{
    public sealed class Command : IRequest<bool>
    {
        public readonly Guid Id;

        public Command(Guid id)
        {
            Id = id;
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
            var recordToDelete = await _itemRepository.GetById(request.Id, cancellationToken: cancellationToken);

            _itemRepository.Remove(recordToDelete);
            return await _unitOfWork.CommitChanges(cancellationToken) >= 1;
        }
    }
}