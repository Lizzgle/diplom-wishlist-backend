using AutoMapper;
using Event.Contracts;
using MediatR;

namespace Event.Application.Usecases.Invitations.Queries.GetByUserId;

public class GetByUserIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetByUserIdRequest, GetByUserIdResponse>
{
    public async Task<GetByUserIdResponse> Handle(GetByUserIdRequest request, CancellationToken cancellationToken)
    {
        var invitations = await unitOfWork.InvitationRepository.GetInvitationsByUserIdAsync(request.UserId, cancellationToken);

        var invitationDtos = mapper.Map<List<InvitationByUserId>>(invitations);
        
        return new GetByUserIdResponse() { UserId = request.UserId, Invitations = invitationDtos };
    }
}