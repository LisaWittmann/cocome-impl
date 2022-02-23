using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(CocomeStore.Areas.Identity.IdentityHostingStartup))]
namespace CocomeStore.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}