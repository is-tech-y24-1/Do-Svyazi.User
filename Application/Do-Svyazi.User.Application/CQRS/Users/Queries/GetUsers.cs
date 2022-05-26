using AutoMapper;
using AutoMapper.QueryableExtensions;
using Do_Svyazi.User.Application.Abstractions.DbContexts;
using Do_Svyazi.User.Domain.Users;
using Do_Svyazi.User.Dtos.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Do_Svyazi.User.Application.CQRS.Users.Queries;

public static class GetUsers
{
    public record Query : IRequest<Response>;
    public record Response(List<MessengerUserDto> Users);

    public class Handler : IRequestHandler<Query, Response>
    {
        private readonly IUsersAndChatDbContext _context;
        private readonly IMapper _mapper;

        public Handler(IUsersAndChatDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            var result = await _context
                .Users
                .ProjectTo<MessengerUserDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken: cancellationToken);

            return new Response(result);
        }
    }
}