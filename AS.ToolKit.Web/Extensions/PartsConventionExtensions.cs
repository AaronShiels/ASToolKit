using System;
using System.Composition.Convention;

namespace AS.ToolKit.Web.Extensions
{
    public static class PartsConventionExtensions
    {
        public static ConventionBuilder WithPartsFolderConventions(this ConventionBuilder conventions)
        {
            return WithPartsFolderConventions(conventions, t => true);
        }

        public static ConventionBuilder WithPartsFolderConventions(
            this ConventionBuilder conventions,
            Func<Type, bool> additionalPredicate)
        {
            if (conventions == null) throw new ArgumentNullException("conventions");
            if (additionalPredicate == null) throw new ArgumentNullException("additionalPredicate");

            conventions.ForTypesMatching(x => x.IsPublic &&
                                              x.Namespace != null &&
                                              x.Namespace.Contains(".Parts") &&
                                              additionalPredicate(x))
                       .Export()
                       .ExportInterfaces();

            return conventions;
        }
    }
}