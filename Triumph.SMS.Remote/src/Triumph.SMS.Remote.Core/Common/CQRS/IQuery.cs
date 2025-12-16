using MediatR;

namespace Triumph.SMS.Remote.Core.Common.CQRS;

public interface IQuery<TResponse> : IRequest<TResponse> { }