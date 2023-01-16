namespace EmployeeManagement.Domain.Clients.Features;

using EmployeeManagement.Domain.Clients.Services;
using EmployeeManagement.Services;
using SharedKernel.Exceptions;
using MediatR;

public static class DeleteClient
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
        private readonly IClientRepository _clientRepository;
        private readonly IUnitOfWork _unitOfWork;

        public Handler(IClientRepository clientRepository, IUnitOfWork unitOfWork)
        {
            _clientRepository = clientRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
        {
            var recordToDelete = await _clientRepository.GetById(request.Id, cancellationToken: cancellationToken);

            _clientRepository.Remove(recordToDelete);
            return await _unitOfWork.CommitChanges(cancellationToken) >= 1;
        }
    }
}