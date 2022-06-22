using Do_Svyazi.User.Domain.Authenticate;
using MediatR;

namespace Do_Svyazi.User.Application.CQRS.Authenticate.Commands;

public record Register(RegisterModel model)
    : IRequest;