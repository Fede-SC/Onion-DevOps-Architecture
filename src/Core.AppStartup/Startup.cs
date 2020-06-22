using System;
using Microsoft.Extensions.DependencyInjection;
using StructureMap;

namespace ClearMeasure.OnionDevOpsArchitecture.Core.AppStartup
{
    public class Startup
    {
        public IServiceProvider Start(IServiceCollection services)
        {
            // C'è un meccanismo di container che permette di tenere traccia delle interfacce che vogliamo usare e configurarle per dire quale 
            // implementazione andare ad usare. In questo caso l'interfaccia IStartupTask, quindi tutte le cose che voglio eseguire quando parte un
            // progetto.
            var container = new ContainerInitializer().GetContainer();
            container.Populate(services);
            // Possiamo notare che c'è un ciclo che chiede a questo oggetto dove vengono registrati tutti gli oggetti che andremo ad utilizzare
            // di darmi gli oggetti che sono IStartupTask così posso eseguirci il Run
            foreach (var task in container.GetAllInstances<IStartupTask>())
            {
                task.Run();
            }
            // Potendoci poi esegire il run questo ci permette di proseguire e di far partire subito il codice

            return container.GetInstance<IServiceProvider>();
        }
    }
}