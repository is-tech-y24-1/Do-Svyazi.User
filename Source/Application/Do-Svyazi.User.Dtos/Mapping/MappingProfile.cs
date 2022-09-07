using AutoMapper;
using Do_Svyazi.User.Domain.Chats;
using Do_Svyazi.User.Domain.Roles;
using Do_Svyazi.User.Domain.Users;
using Do_Svyazi.User.Dtos.Chats;
using Do_Svyazi.User.Dtos.Roles;
using Do_Svyazi.User.Dtos.Users;

namespace Do_Svyazi.User.Dtos.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Chat, MessengerChatDto>()
            .ForMember(
                chatDto => chatDto.Users,
                conf => conf
                    .MapFrom(chat => chat.Users
                        .Select(user => user.MessengerUserId)));

        CreateMap<MessengerUser, MessengerUserDto>();
        CreateMap<Role, RoleDto>();
    }
}