using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace PetFinder {
    public class Program {
        public static void Main(string[] args) {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => {
                    webBuilder.UseUrls(new string[] { "https://localhost:6001", "http://localhost:6000" });
                    webBuilder.UseStartup<Startup>();
                });
    }
}
