using Microsoft.Extensions.Configuration;
using System.IO;

namespace GetVisitantsBehaviorRobot.Models
{
    public class Utility
    {
        public static string GetConnectionString(string key)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddEnvironmentVariables()
                .AddUserSecrets<Program>();

            var configuration = builder.Build();
            var configValue = configuration[key];

            return configValue;
        }
    }
}
