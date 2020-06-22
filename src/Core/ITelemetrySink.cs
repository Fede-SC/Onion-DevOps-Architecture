using System;
using System.Collections.Generic;
using System.Text;

namespace ClearMeasure.OnionDevOpsArchitecture.Core
{
    // Telemetry è quel servizio di applicativi che ha il compito di loggare ciò che succede
    // interfaccia che dal nome registra una chiamata, quando verrà fatta una chiamata io dovrò loggarla e questa interfaccia mi vincola dicendomi
    // che dovrò loggare la richiesta e la risposta
    public interface ITelemetrySink
    {
        void RecordCall<TResponse>(IRequest<TResponse> request, TResponse response);
    }
}
