namespace EmployeeManagement.Domain.Clients.Features;

using EmployeeManagement.Domain.Clients.Services;
using EmployeeManagement.Domain.Clients;
using EmployeeManagement.Domain.Clients.Dtos;
using EmployeeManagement.Services;
using SharedKernel.Exceptions;
using MapsterMapper;
using MediatR;

public static class AddClient
{
    public sealed class Command : IRequest<ClientDto>
    {
        public readonly ClientForCreationDto ClientToAdd;

        public Command(ClientForCreationDto clientToAdd)
        {
            ClientToAdd = clientToAdd;
        }
    }

    public sealed class Handler : IRequestHandler<Command, ClientDto>
    {
        private readonly IClientRepository _clientRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public Handler(IClientRepository clientRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _clientRepository = clientRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ClientDto> Handle(Command request, CancellationToken cancellationToken)
        {
            var client = Client.Create(request.ClientToAdd);
            await _clientRepository.Add(client, cancellationToken);

            await _unitOfWork.CommitChanges(cancellationToken);

            var clientAdded = await _clientRepository.GetById(client.Id, cancellationToken: cancellationToken);
            return _mapper.Map<ClientDto>(clientAdded);
        }
    }
}