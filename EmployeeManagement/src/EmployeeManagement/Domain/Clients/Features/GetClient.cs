namespace EmployeeManagement.Domain.Clients.Features;

using EmployeeManagement.Domain.Clients.Dtos;
using EmployeeManagement.Domain.Clients.Services;
using SharedKernel.Exceptions;
using MapsterMapper;
using MediatR;

public static class GetClient
{
    public sealed class Query : IRequest<ClientDto>
    {
        public readonly Guid Id;

        public Query(Guid id)
        {
            Id = id;
        }
    }

    public sealed class Handler : IRequestHandler<Query, ClientDto>
    {
        private readonly IClientRepository _clientRepository;
        private readonly IMapper _mapper;

        public Handler(IClientRepository clientRepository, IMapper mapper)
        {
            _mapper = mapper;
            _clientRepository = clientRepository;
        }

        public async Task<ClientDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var result = await _clientRepository.GetById(request.Id, cancellationToken: cancellationToken);
            return _mapper.Map<ClientDto>(result);
        }
    }
}