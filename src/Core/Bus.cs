using System;
using System.Diagnostics;

namespace ClearMeasure.OnionDevOpsArchitecture.Core
{
    //Code adapted from MvcContrib (https://archive.codeplex.com/?p=mvccontrib)
    //Some code adapted from Mediatr (https://github.com/jbogard/MediatR)
    public delegate object SingleInstanceFactory(Type serviceType);

    // C'è una prima implementazione perché questo applicativo sostanzialmente gestisce tutte le richieste tramite messaggi
    // L'applicativo spedisce una richiesta, una qualche richiesta che poi verrà gestita sulla base di che cosa è la richiesta. 
    public partial class Bus
    {
        private readonly SingleInstanceFactory _singleInstanceFactory;
        private readonly ITelemetrySink _telemetrySink;

        private Bus()
        {
        }

        public Bus(SingleInstanceFactory singleInstanceFactory, ITelemetrySink telemetrySink)
        {
            _singleInstanceFactory = singleInstanceFactory;
            _telemetrySink = telemetrySink;
        }

        // Ricevere una request ed eseguire correttamente il comando giusto sulla base della request che ho ricevuto
        // Quindi mi è arrivata la richiesta di eseguire un comando
        public virtual TResponse Send<TResponse>(IRequest<TResponse> request)
        {
            Trace.WriteLine(message: string.Format("Message sent: {0}", request.GetType().FullName));
            // Con la reflection viene creato in maniera dinamica il giusto oggetto per gestire questa richiesta
            var defaultHandler = GetHandler(request);

            // Una volta che ho l'oggetto che gestisce la richiesta eseguo il comando e lo passo al mio chiamante per poter gestire l'esecuzione
            var result = defaultHandler.Handle(request);
            // Primo richiamo alla telemetria. Voglio che la telemetria tenga traccia che è stata fatta la request con questi specifici dati ma non mi
            // interessa di come è implementata e quindi la richiamo solo
            _telemetrySink.RecordCall<TResponse>(request, result);

            return result;
        }

        // Con la reflection viene creato in maniera dinamica il giusto oggetto per gestire questa richiesta
        private RequestHandler<TResponse> GetHandler<TResponse>(IRequest<TResponse> request)
        {
            var handlerType = typeof(IRequestHandler<,>).MakeGenericType(request.GetType(), typeof(TResponse));
            var wrapperType = typeof(RequestHandler<,>).MakeGenericType(request.GetType(), typeof(TResponse));
            object handler;
            try
            {
                handler = _singleInstanceFactory(handlerType);

                if (handler == null)
                    throw new InvalidOperationException(
                        "Handler was not found for request of type " + request.GetType());
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Handler was not found for request of type " + request.GetType(),
                    e);
            }

            var wrapperHandler =
                Activator.CreateInstance(wrapperType, handler);
            return (RequestHandler<TResponse>) wrapperHandler;
        }

        private abstract class RequestHandler<TResult>
        {
            public abstract TResult Handle(IRequest<TResult> message);
        }

        private class RequestHandler<TCommand, TResult> : RequestHandler<TResult> where TCommand : IRequest<TResult>
        {
            private readonly IRequestHandler<TCommand, TResult> _inner;

            public RequestHandler(IRequestHandler<TCommand, TResult> inner)
            {
                _inner = inner;
            }

            public override TResult Handle(IRequest<TResult> message)
            {
                return _inner.Handle((TCommand) message);
            }
        }
    }
}