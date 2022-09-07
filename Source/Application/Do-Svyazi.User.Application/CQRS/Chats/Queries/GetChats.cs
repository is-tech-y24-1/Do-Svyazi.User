using Do_Svyazi.User.Dtos.Chats;
using MediatR;

namespace Do_Svyazi.User.Application.CQRS.Chats.Queries;

public record GetChats
    : IRequest<IReadOnlyCollection<MessengerChatDto>>;