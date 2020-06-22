using ClearMeasure.OnionDevOpsArchitecture.IntegrationTests.DataAccess;
using StructureMap;

namespace ClearMeasure.OnionDevOpsArchitecture.IntegrationTests
{
    // Cassetto di Utility che va a pulire il database in maniera tale che ogni volta che io eseguo il ExpenseReportMappingTester non ho dati sporchi
    // appesi nel db
    public class DatabaseTester
    {
        private static bool _dependenciesRegistered;
        private static readonly object Lock = new object();
        private static IContainer _container;

        public void Clean()
        {
            EnsureDependenciesRegistered();
            new DatabaseEmptier().DeleteAllData();
        }

        private static void EnsureDependenciesRegistered()
        {
            if (!_dependenciesRegistered)
            {
                lock (Lock)
                {
                    if (!_dependenciesRegistered)
                    {
                        Initialize();
                        _dependenciesRegistered = true;
                    }
                }
            }
        }

        private static void Initialize()
        {
            var container = new Container(new StructureMapRegistry());
            _container = container;
        }

        public IContainer GetContainer()
        {
            EnsureDependenciesRegistered();
            return _container;
        }
    }
}