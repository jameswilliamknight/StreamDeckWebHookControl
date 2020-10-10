using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace StreamDeckWebHookControl
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            var configuration = BuildConfiguration();
            var serviceProvider = SetupIoC(configuration);

            // https://ifttt.com/maker_webhooks --> "Documentation"
            var iftttKey = serviceProvider.GetRequiredService<IOptions<AppConfig>>().Value.IftttKey;

            //var device = "bedroom_window_light";
            var device = "bedroom_door_light";

            var url = new Uri($"https://maker.ifttt.com/trigger/{device}/with/key/{iftttKey}");
            var client = new HttpClient();
            var jsonObj = JToken.Parse("{ \"value1\" : \"20\" }"); // todo, brightness from args
            var content = new StringContent(jsonObj.ToString(), Encoding.UTF8, "application/json");
            var result = await client.PostAsync(url, content);
        }

        private static IServiceProvider SetupIoC(IConfiguration configuration)
        {
            var appConfig = configuration.GetSection("AppConfig");
            return new ServiceCollection()
                .AddOptions()
                .Configure<AppConfig>(appConfig)
                .BuildServiceProvider();
        }

        private static IConfiguration BuildConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }
    }
}
