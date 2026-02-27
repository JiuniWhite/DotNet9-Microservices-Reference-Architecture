namespace Provider.Application.UseCases.Abstractions.UseCases
{
    public interface IInteractorUseCase<TRequest, TResponse>
    {
        Task<TResponse> InteractAsync(string user, TRequest request, CancellationToken cancellationToken);
       
    }
}

