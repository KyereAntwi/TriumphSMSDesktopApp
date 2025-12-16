using MediatR;

namespace Triumph.SMS.Remote.Core.Common.CQRS;

public interface ICommand<TResponse> : IRequest<TResponse> { }

public interface ICommand : ICommand<Unit>, IRequest { }