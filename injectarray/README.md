# injectarray with ASP.NET Core

In this project the default IoC mechanism built in to ASP.NET Core is used to inject an array of services into the Controller.

* ASP.NET Code IoC does not have support built in for injecting arrays

* It does however, support injecting an IEnumerable&lt;T&gt;.
Notice that the service registration process in Startup.cs simply registers the IFilter implementations.
The ValuesController takes IEnumerable<IFilter&lt;string&gt;> as a parameter.

* Configure ctor params - in order to provide selective injection for ctor params I needed to do 2 things:
1. Create a provider and inject that into the controller ctor - see LowerValuesController and UpperValuesController
```csharp
public LowerValuesController(LowerFilterProvider filterProvider, ILogger<LowerValuesController> logger)
{
    Filters = filterProvider.Filters;
```

2. Inject the explicit types into the provider ctor - see LowerFilterProvider and UpperFilterProvider
```csharp
using System.Collections.Generic;
using injectarray.Filters;

namespace injectarray.Controllers
{
    public class LowerFilterProvider : IFilterProvider<string>
    {
        public IEnumerable<IFilter<string>> Filters { get; }

        public LowerFilterProvider(LowerCaseFilter lowerCaseFilter, CountCharsFilter countCharsFilter)
        {
            Filters = new IFilter<string>[]
            {
                lowerCaseFilter,
                countCharsFilter
            };
        }
    }
}
```

* There are no named or keyed registration capabilities that I could find.

* Note that the service add methods support the use of a _factory_ function. This could accomplish the same thing but
still requires a unique symbol to inject into the controller ctor to accomplish what we are trying to do here.

With that in mind I prefer the explicit provider class approach.

```csharp
var mongoConnection = //...
var efConnection = //...
var otherConnection = //...
services.AddTransient<IMyFactory>(
             s => new MyFactoryImpl(
                 mongoConnection, efConnection, otherConnection, 
                 s.GetService<ISomeDependency1>(), s.GetService<ISomeDependency2>())));
```
From [How to register multiple implementations of the same interface in Asp.Net Core?](http://stackoverflow.com/questions/39174989/how-to-register-multiple-implementations-of-the-same-interface-in-asp-net-core)
