using MediatR;

namespace Do_Svyazi.User.Application.CQRS;

public interface ICommandHandler<in TRequest, TResponse>
    : IRequestHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
}