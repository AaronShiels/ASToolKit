using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Composition.Convention;
using System.Composition.Hosting;
using Alt.Composition;
using Alt.Composition.Convention;
using Alt.Composition.Hosting;

namespace AS.ToolKit.Web
{
    public class CompositionConfig
    {
        public static void Configure()
        {
            var conventions = new ConventionBuilder()
                .WithMvcConventions();

            var container = new ContainerConfiguration()
            .WithDefaultConventions(conventions)
            .WithAssembly(typeof(MvcApplication).Assembly)
            .CreateContainer();

            MvcCompositionProvider.Initialize(container);
            CompositionFilterProvider.Install(FilterProviders.Providers);
            ImportCapableFilterAttributeFilterProvider.Install(FilterProviders.Providers);
        }
    }
}