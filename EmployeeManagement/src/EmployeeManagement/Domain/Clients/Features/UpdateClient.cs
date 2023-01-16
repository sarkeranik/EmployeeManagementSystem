namespace EmployeeManagement.Domain.Clients.Features;

using EmployeeManagement.Domain.Clients;
using EmployeeManagement.Domain.Clients.Dtos;
using EmployeeManagement.Domain.Clients.Services;
using EmployeeManagement.Services;
using SharedKernel.Exceptions;
using MapsterMapper;
using MediatR;

public static class UpdateClient
{
    public sealed class Command : IRequest<bool>
    {
        public readonly Guid Id;
        public readonly ClientForUpdateDto UpdatedClientData;

        public Command(Guid id, ClientForUpdateDto updatedClientData)
        {
            Id = id;
            UpdatedClientData = updatedClientData;
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
            var clientToUpdate = await _clientRepository.GetById(request.Id, cancellationToken: cancellationToken);

            clientToUpdate.Update(request.UpdatedClientData);
            _clientRepository.Update(clientToUpdate);
            return await _unitOfWork.CommitChanges(cancellationToken) >= 1;
        }
    }
}