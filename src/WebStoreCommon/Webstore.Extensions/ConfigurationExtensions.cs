using Microsoft.Extensions.Configuration;

namespace WebStore.Extensions
{
    public static class ConfigurationExtensions
    {
        public static string GetSqlConnectionString(this IConfiguration configuration, string environmentVariableName)
        {
            return Environment.GetEnvironmentVariable(environmentVariableName).Equals("Development")
                ? configuration.GetConnectionString("WebstoreDb") : configuration.GetConnectionString("WebstoreDbDocker");
        }

        public static IConfiguration GetRabbitMqConfiguration(this IConfiguration configuration, string configurationName)
        {
            var rabbitMqConfiguration = configuration.GetSection(configurationName);

            if (Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT").Equals("Development"))
            {
                rabbitMqConfiguration["hostName"] = "localhost";
            }

            return rabbitMqConfiguration;
        }
    }
}
