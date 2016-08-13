# Ninject.Extensions.AutoBindable [![Build status](https://ci.appveyor.com/api/projects/status/hg9kmq7bww02k6px/branch/master?svg=true)](https://ci.appveyor.com/project/robbaman/ninject-extensions-autobindable/branch/master) [![NuGet version](https://badge.fury.io/nu/Ninject.Extensions.AutoBindable.svg)](https://badge.fury.io/nu/Ninject.Extensions.AutoBindable)

Enables automatic binding of service types to implementation types using a simple marker interface.

## Sample usage


### Mark a service interface

Simply mark any service interface as being AutoBindable:

```CSharp
public interface IDoesSomething : Ninject.Extensions.AutoBindable.IAutoBindable {
	void ImportantFunction();
}
```

### Implement the service

Next provide one or more implementations of the interface service:

```CSharp
public class SomethingProvider : IDoesSomething {
	public void ImportantFunction() {
		// Do important stuff!
	}
}
```

### Use the service interface in any class obtained from a Ninject Kernel

In the case where only one class implements `IDoesSomething`:

```CSharp
public class SomeConsumer {
	public SomeConsumer(IDoesSomething doesSomethingInstance) {
		// Use the IDoesSomething instance
	}
}
```

Or, in the case where are multiple classes implementing `IDoesSomething`:

```CSharp
public class SomeConsumer {
	public SomeConsumer(IDoesSomething[] doesSomethingInstances) {
		// Use the IDoesSomething instances here
	}
}
```