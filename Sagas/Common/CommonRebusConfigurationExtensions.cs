using Rebus.Config;
using Serilog;
// ReSharper disable ArgumentsStyleLiteral

namespace Common
{
    public static class CommonRebusConfigurationExtensions
    {
        public static RebusConfigurer ConfigureEndpoint(this RebusConfigurer configurer, EndpointRole role)
        {
            configurer
                .Logging(l => l.Serilog(Log.Logger))
                .Transport(t =>
                {
                    if (role == EndpointRole.Client)
                    {
                        t.UseSqlServerAsOneWayClient(Config.ConnectionString("RebusDatabase"));
                    }
                    else
                    {
                        t.UseSqlServer(Config.ConnectionString("RebusDatabase"), Config.AppSetting("QueueName"));

                    }
                })
                .Subscriptions(s =>
                {
                    var subscriptionsTableName = Config.AppSetting("SubscriptionsTableName");

                    s.StoreInSqlServer(Config.ConnectionString("RebusDatabase"), subscriptionsTableName, isCentralized: true);
                })
                .Sagas(s =>
                {
                    if (role != EndpointRole.SagaHost) return;

                    var dataTableName = Config.AppSetting("SagaDataTableName");
                    var indexTableName = Config.AppSetting("SagaIndexTableName");

                    // store sagas in SQL Server to make them persistent and survive restarts
                    s.StoreInSqlServer(Config.ConnectionString("RebusDatabase"), dataTableName, indexTableName);
                });
             

            return configurer;
        }
    }
}
