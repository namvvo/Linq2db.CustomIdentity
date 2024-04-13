using LinqToDB.Configuration;

namespace BlazorIdWithLinq2db
{
    public class Linq2dbSettings : ILinqToDBSettings
    {
        private const string msDefaultProviderName = "SqlServer";
        public readonly IConnectionStringSettings mConnectionStringSettings;
        public Linq2dbSettings(WebApplicationBuilder builder)
        {
            // Figure out the database name from the connection string.
            var sDBConnection = builder.Configuration["ConnectionStrings:DefaultConnection"];
            var arrDBConnName = sDBConnection.Split("Database=");
            var sDBName = arrDBConnName.Length > 1 ? arrDBConnName[1] : "DemoDB";
            var nEndIndx = sDBName.IndexOf(";");
            if (nEndIndx > 0)
            {
                sDBName = sDBName.Substring(0, nEndIndx);
            }
            var sProviderName = builder.Configuration["Authentication:Linq2db:ProviderName"] ?? "";
            if (sProviderName.Length == 0)
            {
                sProviderName = msDefaultProviderName;
            }

            mConnectionStringSettings = new Data.ConnectionStringSettings
            {
                Name = sDBName,
                ProviderName = sProviderName,
                ConnectionString = sDBConnection
            };

            //builder.Services.AddLinqToDBContext
        }

        public IEnumerable<IDataProviderSettings> DataProviders => Enumerable.Empty<IDataProviderSettings>();

        public string DefaultConfiguration => mConnectionStringSettings.Name;

        public string DefaultDataProvider => "SqlServer";

        public IEnumerable<IConnectionStringSettings> ConnectionStrings
        {
            get
            {
                yield return mConnectionStringSettings;
            }
        }
    }
}
