using System.Reflection;

namespace ClearMeasure.OnionDevOpsArchitecture.Core
{
    // Interfaccia dedicata ai dati di configurazione
    public interface IDataConfiguration
    {
        string GetConnectionString(); // metodo che ottiene la connection string
        string GetValue(string key, Assembly configAssembly); // metodo che ottiene un valore generico di un settaggio
    }
}