# injectarray with Autofac - Keyed

In this project the Autofac IoC keyed mechanism is used to inject an array of services into the Controllers.

* This project uses the Keyed mechanism of Autofac.
See [Keyed Services](http://autofac.readthedocs.io/en/latest/advanced/keyed-services.html#keyed-services) and 
[Keyed Service Lookup](http://autofac.readthedocs.io/en/latest/resolve/relationships.html#keyed-service-lookup-iindex-x-b).

* It relies on support for injecting an IEnumerable&lt;T&gt;.
Notice that the service registration process in Startup.cs registers the IFilter implementations as keyed registrations for
the LowerValuesController and UpperValuesController are keyed and their ctors take IIndex as a param.
```cs
    // Keyed registrations in Startup.cs

    /* For LowerValuesController */
    builder.RegisterType<CountCharsFilter>().Keyed<IFilter<string>>(LowerValuesController.Key);
    builder.RegisterType<LowerCaseFilter>().Keyed<IFilter<string>>(LowerValuesController.Key);

    /* For UpperValuesController */
    builder.RegisterType<CountCharsFilter>().Keyed<IFilter<string>>(UpperValuesController.Key);
    builder.RegisterType<UpperCaseFilter>().Keyed<IFilter<string>>(UpperValuesController.Key);
```

```cs
    public class UpperValuesController : Controller
    {
        public const string Key = "upper";

        IEnumerable<IFilter<string>> Filters { get; }

        public UpperValuesController(IIndex<string, IEnumerable<IFilter<string>>> filters, ILogger<ValuesController> logger)
        {
            Filters = filters[UpperValuesController.Key]; // This is the Autofac keyed injected type
```
The ValuesController registrations are not keyed and simply takes IEnumerable<IFilter&lt;string&gt;> as a parameter.
```cs
    /* For ValuesController */
    builder.RegisterType<CountCharsFilter>().As<IFilter<string>>();
    builder.RegisterType<UpperCaseFilter>().As<IFilter<string>>();
    builder.RegisterType<LowerCaseFilter>().As<IFilter<string>>();
```

```cs
    public ValuesController(IEnumerable<IFilter<string>> filters, ILogger<ValuesController> logger)
    {
        Filters = filters;
```

* The configuration used is as documented at [Application Integration ASP.NET Core](http://autofac.readthedocs.io/en/latest/integration/aspnetcore.html).
See the [Startup](Startup.cs) class for the changes made there.
