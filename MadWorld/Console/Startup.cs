using System;
using System.IO;
using Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Console
{
    public sealed class Startup
    {
        private const bool Development = true;
        private static Startup _startup;
        private string _connectionMadWorld;
        public DataInserter Inserter { get; private set; }
        private Startup() { }
        public static Startup Create()
        {
            if (_startup == null)
            {
                _startup = new Startup();
            }

            return _startup;
        }

        public void Load()
        {
            string setting = "appsettings.json";

            if (Development)
            {
                setting = "appsettings.Development.json";
            }

            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(AppContext.BaseDirectory + "../../../../API"))
                .AddJsonFile(setting, true, true)
                .AddEnvironmentVariables();

            var config = builder.Build();
            _connectionMadWorld = config["ConnectionStrings:MadWorldContext"];

            CreateMadWorldContext();
        }

        private void CreateMadWorldContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<MadWorldContext>();
            optionsBuilder.UseNpgsql(_connectionMadWorld);

            MadWorldContext context = new MadWorldContext(optionsBuilder.Options);

            Inserter = new(context);
        }
    }
}
