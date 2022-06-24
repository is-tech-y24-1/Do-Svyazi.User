using AutoMapper;
using Do_Svyazi.User.Application.CQRS.Handlers;
using Do_Svyazi.User.Application.CQRS.Roles.Queries;
using Do_Svyazi.User.Application.DbContexts;
using Do_Svyazi.User.Domain.Exceptions;
using Do_Svyazi.User.Domain.Users;
using Do_Svyazi.User.Dtos.Roles;
using Microsoft.EntityFrameworkCore;

namespace Do_Svyazi.User.Application.CQRS.Roles.Handlers;

public class RolesQueryHandler
    : IQueryHandler<GetRoleByUserIdQuery, RoleDto>
{
    private readonly IDbContext _context;
    private readonly IMapper _mapper;

    public RolesQueryHandler(IDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<RoleDto> Handle(GetRoleByUserIdQuery request, CancellationToken cancellationToken)
    {
        ChatUser chatUser =
            await _context.ChatUsers
                .Include(chatUser => chatUser.Role)
                .FirstOrDefaultAsync(user => user.MessengerUserId == request.userId && user.ChatId == request.chatId, cancellationToken: cancellationToken) ??
            throw new Do_Svyazi_User_NotFoundException(
                $"Chat user with userId = {request.userId} and chatId = {request.chatId} to get user role was not found");

        return _mapper.Map<RoleDto>(chatUser.Role);
    }
}