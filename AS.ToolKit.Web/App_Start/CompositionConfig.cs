using System.Web.Mvc;
using System.Composition.Convention;
using System.Composition.Hosting;
using AS.ToolKit.Data;
using AS.ToolKit.Web.Extensions;
using Alt.Composition.Convention;
using Alt.Composition.Hosting;

namespace AS.ToolKit.Web.App_Start
{
    public class CompositionConfig
    {
        public static void Configure()
        {
            var conventions = new ConventionBuilder()
                .WithMvcConventions()
                .WithPartsFolderConventions();

            var container = new ContainerConfiguration()
            .WithDefaultConventions(conventions)
            .WithApplicationSettings()
            .WithAssembly(typeof(MvcApplication).Assembly)
            .WithAssembly(DataParts.Assembly)
            .CreateContainer();

            MvcCompositionProvider.Initialize(container);
            CompositionFilterProvider.Install(FilterProviders.Providers);
            ImportCapableFilterAttributeFilterProvider.Install(FilterProviders.Providers);
        }
    }
}