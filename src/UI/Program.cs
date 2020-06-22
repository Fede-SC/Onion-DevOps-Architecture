using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace ClearMeasure.OnionDevOpsArchitecture.UI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = CreateWebHostBuilder(args);
            var host = builder.Build();
            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            // Comando che precostruisce avvia l'hosting web e applicativo e offre tutta una serie di metodi che permettono di configurare come
            // eseguire questo server. Ho pochi oggetti che mi fanno creare l'hoster principale e poi con l'intellisense a tutta una serie di comandi
            // utili
            return WebHost.CreateDefaultBuilder(args)
                // comando che istruisce l'oggetto di telemetry client di andarsi a leggere appsettings per andarsi a leggere le informazioni giuste
                .UseApplicationInsights() 
                // asp.net core istruisce di dover utilizzare la classe Startup per gestire l'avvio del progetto
                .UseStartup<Startup>();
        }
    }
}
