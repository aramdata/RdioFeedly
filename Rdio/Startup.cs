using Hangfire;
using Hangfire.Mongo;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute("RdioStartup",typeof(Rdio.Startup))]
namespace Rdio
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //app.MapSignalR();


            //Must Manual Create Database In SQL Server
            //GlobalConfiguration.Configuration.UseSqlServerStorage("HangFireDB");
            GlobalConfiguration.Configuration.UseMongoStorage("mongodb://localhost:27017", "rdiofeedlyhangfire");

            app.UseHangfireServer();
            app.UseHangfireDashboard();
            ConfigureAuth(app);

            var rssService = new Service.RssService();
            RecurringJob.AddOrUpdate(() => rssService.RssFetcherManager(), Cron.MinuteInterval(5));

            var crawlService=new Service.CrawlerService();
            RecurringJob.AddOrUpdate(() => crawlService.CrawlManager(), Cron.MinuteInterval(5));

            var crawlLinkService = new Service.CrawlerService();
            RecurringJob.AddOrUpdate(() => crawlLinkService.CrawlLinkManager(), Cron.MinuteInterval(5));
        }
    }
}
