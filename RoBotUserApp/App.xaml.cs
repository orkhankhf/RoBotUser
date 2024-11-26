using Autofac;
using Autofac.Extensions.DependencyInjection;
using Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RoBotApp.Modules;
using System.IO;
using System.Windows;

namespace RoBotApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; }
        public IConfiguration Configuration { get; private set; }

        public App()
        {
            // Load configuration from appsettings.json
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            // Create Autofac container
            var containerBuilder = new ContainerBuilder();
            containerBuilder.Populate(serviceCollection); // Populate services registered in .NET Core DI
            containerBuilder.RegisterModule(new RepoServiceModule()); // Register the custom Autofac module
            var container = containerBuilder.Build();

            // Create the service provider for use in the application
            ServiceProvider = new AutofacServiceProvider(container);
        }

        private IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // Get connection string from appsettings.json
            string connectionString = Configuration.GetConnectionString("DefaultConnection");

            // Configure DbContext with the connection string
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));

            return services.BuildServiceProvider();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
        }
    }
}
