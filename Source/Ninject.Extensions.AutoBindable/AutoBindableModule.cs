using System;
using System.Linq;
using Ninject.Modules;

namespace Ninject.Extensions.AutoBindable {
    public class AutoBindableModule : NinjectModule {
        public override void Load() {
            // Get the IAutoBindable Type.
            var autoBindInterface = typeof(IAutoBindable);
            // Get all the types loaded in the current AppDomain
            var allTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(i => i.GetTypes()).ToArray();
            // Find all interfaces that extend IAutoBindable
            var autoBindInterfaces = allTypes.Where(i => i != autoBindInterface && i.IsInterface && autoBindInterface.IsAssignableFrom(i));
            // Find the implementations of these inheriting interfaces.
            foreach (var autoBind in autoBindInterfaces) {
                var implementations = allTypes.Where(i => i.IsClass && !i.IsAbstract && autoBind.IsAssignableFrom(i));
                foreach (var implementation in implementations) {
                    // Bind the implementations automatically
                    Bind(autoBind).To(implementation);
                }
            }

        }
    }
}
