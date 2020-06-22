using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ClearMeasure.OnionDevOpsArchitecture.UI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // services.AddMvc() serve ad aggiungere questi servizi ad mvc e averli disponibili nell'applicativo
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            // C'è la tecnica di spostare le logiche di avvio in un progetto ad hoc perché in questo caso viene popolato questo services con una
            // tecnica custom 
            var provider = new Core.AppStartup.Startup().Start(services);
            return provider;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // Una volta che asp.net core ha inizializzato tutto il sistema c'è una configurazione web oriented 
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // Permette di avere risposta ai file statici di wwwroot
            app.UseStaticFiles();

            // Ci permette di sapere, se siamo in ambiente di sviluppo o in un ambiente di produzione
            if (env.IsDevelopment())
                // Se siamo in ambiente di sviluppo attiviamo l'opzione di vedere gli errori in maniera più parlante, errori che in produzione si
                // tende a nascondere per evitare falle
                app.UseDeveloperExceptionPage();
            else
                // Se si è in produzione gli errori saranno offuscati e in più obbliga ad usare l'https
                app.UseHsts();

            // Se viene fatta la richiesta Http fa il redirect in Https
            app.UseHttpsRedirection();

            // UseMvc attiva quei servizi che in services.AddMvc() avevamo solo aggiunto
            app.UseMvc();
        }
    }
}