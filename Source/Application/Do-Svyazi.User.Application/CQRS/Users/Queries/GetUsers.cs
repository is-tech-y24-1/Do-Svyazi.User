using AutoMapper;
using Do_Svyazi.User.Application.DbContexts;
using Do_Svyazi.User.Domain.Users;
using Do_Svyazi.User.Dtos.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Do_Svyazi.User.Application.CQRS.Users.Queries;

public record GetUsers : IRequest<IReadOnlyCollection<MessengerUserDto>>
{
    public class Handler : IRequestHandler<GetUsers, IReadOnlyCollection<MessengerUserDto>>
    {
        private readonly IDbContext _context;
        private readonly IMapper _mapper;

        public Handler(IDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IReadOnlyCollection<MessengerUserDto>> Handle(GetUsers request, CancellationToken cancellationToken)
        {
            List<MessengerUser> users = await _context.Users
                .ToListAsync(cancellationToken: cancellationToken);

            return _mapper.Map<IReadOnlyCollection<MessengerUserDto>>(users);
        }
    }
}