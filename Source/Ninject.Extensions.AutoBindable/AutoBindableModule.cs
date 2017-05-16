using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Ninject.Modules;

namespace Ninject.Extensions.AutoBindable {
    public class AutoBindableModule : NinjectModule {
        public override void Load() {
			// Get the IAutoBindable Type.
			var autoBindInterface = typeof(IAutoBindable);
			// Get all the assemblies currently loaded.
			var allTypes = GetAllTypes();

			BindAllTypes(autoBindInterface, allTypes);
		}

		private void BindAllTypes(Type autoBindInterface, List<Type> allTypes) {
			// Find all interfaces that extend IAutoBindable
			var autoBindInterfaces = allTypes.Where(i => i != autoBindInterface && i.IsInterface && autoBindInterface.IsAssignableFrom(i));
			// Find the implementations of these inheriting interfaces.
			foreach (var autoBind in autoBindInterfaces) {
				if (autoBind.IsGenericType) {
					var implementations = from t in allTypes
										  from i in t.GetInterfaces().Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == autoBind)
										  where t.IsClass
												&& !t.IsAbstract
										  select new {
											  Type = t,
											  Interface = i
										  };

					foreach (var implementation in implementations) {
						//var intF = implementation.GetInterfaces().First(j => j.IsGenericType && j.GetGenericTypeDefinition() == autoBind);
						//var genType = intF.GenericTypeArguments[0];
						Bind(autoBind.MakeGenericType(implementation.Interface.GenericTypeArguments)).To(implementation.Type);
					}

				} else {
					var implementations = allTypes.Where(i => i.IsClass && !i.IsAbstract && autoBind.IsAssignableFrom(i));
					foreach (var implementation in implementations) {
						// Bind the implementations automatically
						Bind(autoBind).To(implementation);
					}
				}
			}
		}

		private static List<Type> GetAllTypes() {
			var allAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
			var allTypes = new List<Type>();

			foreach (var assembly in allAssemblies) {
				try {
					// Get all the types in the assembly
					var typesInAssembly = assembly.GetTypes().ToArray();
					allTypes.AddRange(typesInAssembly);
				} catch (Exception ex) {
					// Mey, skip this assembly
					Trace.WriteLine(ex.ToString());
				}
			}

			return allTypes;
		}
	}
}
