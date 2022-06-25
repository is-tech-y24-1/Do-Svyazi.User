using MediatR;

namespace Do_Svyazi.User.Application.CQRS.Handlers;

public interface ICommandHandler<in TRequest, TResponse>
    : IRequestHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
}