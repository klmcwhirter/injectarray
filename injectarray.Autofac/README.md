# injectarray with Autofac

In this project the Autofac IoC mechanism is used to inject an array of services into the Controller.

* This project uses the same approach as the injectarray project (ASP.NET Core IoC).

* It relies on support for injecting an IEnumerable&lt;T&gt;.
Notice that the service registration process in Startup.cs simply registers the IFilter implementations.
The ValuesController takes IEnumerable<IFilter&lt;string&gt;> as a parameter.

* The configuration used is as documented at [Application Integration ASP.NET Core](http://autofac.readthedocs.io/en/latest/integration/aspnetcore.html).
See the [Startup](Startup.cs) class for the changes made there.

* Autofac has an .AsSelf() extension method that allowed me to eliminate some registration duplication as compared to the ASP.NET Core IoC approach.

* Configure ctor params - in order to provide selective injection for ctor params I did 2 things:
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
