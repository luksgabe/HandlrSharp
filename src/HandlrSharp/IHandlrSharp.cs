namespace HandlrSharp
{
    public interface IHandlrSharp
    {
        Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
    }
}