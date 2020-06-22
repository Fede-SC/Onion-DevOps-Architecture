namespace ClearMeasure.OnionDevOpsArchitecture.Core
{
#pragma warning disable CA1040 // Avoid empty interfaces
    // Interfaccia vuota che ci permette di marchiare gli oggetti che vorremmo passare come richieste
    public interface IRequest<out TResponse>
#pragma warning restore CA1040 // Avoid empty interfaces
    {
    }
}