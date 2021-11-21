using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using CommunityToolkit.Mvvm.DependencyInjection;
using Monaco.PathTree.Samples.Wpf.Services;

namespace Monaco.PathTree.Samples.Wpf;

public class Bootstrapper
{
    public virtual void Initialize()
    {
        var services = new ServiceCollection();

        RegisterServices(services);
        RegisterViewModels(services);

        Ioc.Default.ConfigureServices(services.BuildServiceProvider());
    }

    protected virtual void RegisterServices(ServiceCollection services)
    {
        services.AddTransient<IOrganizationService, OrganizationService>();
    }

    protected virtual void RegisterViewModels(ServiceCollection services)
    {
        var vmTypes = GetType()
            .Assembly
            .GetTypes()
            .Where(x => x.Name.EndsWith("ViewModel"))
            .Where(x => !x.IsAbstract && !x.IsInterface);

        foreach (var vmType in vmTypes)
            services.AddTransient(vmType);
    }
}
