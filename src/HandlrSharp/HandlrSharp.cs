using System.Reflection;

namespace HandlrSharp.Core
{
    public class HandlrSharp : IHandlrSharp
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Assembly _assembly;

        public HandlrSharp(IServiceProvider serviceProvider, Assembly assembly)
        {
            _serviceProvider = serviceProvider;
            _assembly = assembly;
        }

        public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
        {
            var requestType = request.GetType();
            var handlerType = typeof(IRequestHandler<,>).MakeGenericType(requestType, typeof(TResponse));
            var handler = _serviceProvider.GetService(handlerType);

            if (handler == null)
                throw new InvalidOperationException($"Handler not found to {requestType.Name}");

            var method = handlerType.GetMethod("Handle");
            var task = (Task<TResponse>)method!.Invoke(handler, new object[] { request, cancellationToken })!;

            return await task;
        }
    }
}