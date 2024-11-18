using Autofac;
using Common;
using Data.Context;
using Data.Interfaces;
using Data.Repositories;
using Data.UnitOfWork;
using Services.Implementations;
using Services.Interfaces;
using System.Reflection;
using Module = Autofac.Module;

namespace RoBotApp.Modules
{
    public class RepoServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Get assemblies
            var apiAssembly = Assembly.GetExecutingAssembly(); // Your current assembly
            var repoAssembly = Assembly.GetAssembly(typeof(AppDbContext)); // Adjust to your DbContext type
            var serviceAssembly = Assembly.GetAssembly(typeof(IGenericService<>)); // Adjust to a known service type

            // Register Repositories
            builder.RegisterAssemblyTypes(repoAssembly)
                .Where(x => x.Name.EndsWith("Repository"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            // Register Services
            builder.RegisterAssemblyTypes(serviceAssembly)
                .Where(x => x.Name.EndsWith("Service"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            // Register Generic Repositories and Services
            builder.RegisterGeneric(typeof(GenericRepository<>))
                .As(typeof(IGenericRepository<>))
                .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(GenericService<>))
                .As(typeof(IGenericService<>))
                .InstancePerLifetimeScope();

            // Register UnitOfWork
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();

            //Register AppSettingsProvider
            builder.RegisterType<AppSettingsProvider>().As<IAppSettingsProvider>().InstancePerLifetimeScope();
        }
    }
}
