using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;

namespace ClearMeasure.OnionDevOpsArchitecture.Core.AppStartup
{
    // Sistema di log che utilizzerà il nostro progetto Core
    // ITelemetrySink viene implementato per usare due metodi di Azure che servono a fare i log su un servizio che si chiama Application Inside, 
    // un sistema di Azure che permette di loggare in una piattaforma che da remoto mi passa tutti quanti i log
    public class TelemetrySink : ITelemetrySink
    {
        public void RecordCall<TResponse>(IRequest<TResponse> request, TResponse response)
        {
            var telemetryClient = new TelemetryClient();
            EventTelemetry telemetry = new EventTelemetry();
            telemetry.Name = request.GetType().Name;
            telemetryClient.TrackEvent(telemetry);
            telemetryClient.TrackTrace(request.GetType().Name + ":- " + request.ToString(), SeverityLevel.Information);
        }
    }
}
