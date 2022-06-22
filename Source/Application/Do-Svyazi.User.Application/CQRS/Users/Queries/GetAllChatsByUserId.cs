using Do_Svyazi.User.Dtos.Chats;
using MediatR;

namespace Do_Svyazi.User.Application.CQRS.Users.Queries;

public record GetAllChatsByUserId(Guid userId)
    : IRequest<IReadOnlyList<MessengerChatDto>>;