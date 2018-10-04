using System;
using System.Data;
using Hangfire;
using Hangfire.MySql;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(HangfireSamlpe.Startup))]

namespace HangfireSamlpe
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            // 接続文字列は環境に応じて変えること
            var constr =
                @"Server=localhost;Port=3306;Uid=root;Pwd=P@ssw0rd;database=hangfireDB;DefaultCommandTimeout=9999;Charset=utf8;Allow User Variables=True";
            GlobalConfiguration.Configuration.UseStorage(
                new MySqlStorage(
                    constr,
                    new MySqlStorageOptions
                    {
                        // !cannot use
                        // TransactionIsolationLevel = IsolationLevel.ReadCommitted,
                        QueuePollInterval = TimeSpan.FromSeconds(15),
                        JobExpirationCheckInterval = TimeSpan.FromHours(1),
                        CountersAggregateInterval = TimeSpan.FromMinutes(5),
                        /* データベースによっては 'Error Code: 1071. Specified key was too long; max key length is 767 bytes'が発生
                         ->Table `SET`, `Value` nvarchar(256) NOT NULL*/
                        // PrepareSchemaIfNecessary = true,
                        DashboardJobListLimit = 50000,
                        TransactionTimeout = TimeSpan.FromMinutes(1),
                        // !cannot use
                        // TablesPrefix = "Hangfire"
                    }));

            app.UseHangfireDashboard();
            app.UseHangfireServer();

        }
    }
}
