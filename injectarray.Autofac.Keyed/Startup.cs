using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using injectarray.Controllers;
using injectarray.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace injectarray
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IContainer ApplicationContainer { get; private set; }

        public IConfigurationRoot Configuration { get; }

        // ConfigureServices is where you register dependencies. This gets
        // called by the runtime before the Configure method, below.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();

            // Create the container builder.
            var builder = new ContainerBuilder();

            /*
            ** Register dependencies, populate the services from
            ** the collection, and build the container. If you want
            ** to dispose of the container at the end of the app,
            ** be sure to keep a reference to it as a property or field.
            */

            /*
            ** The Keyed() extension method coupled with the use of the IIndex type allows for
            ** identifying instances without the use of a provider class.
            **
            ** While not as straightforward as ctor param configuration and causing duplication
            ** in the registration it does get us what we are looking for.
            */

            // Add app Filters

            /* For ValuesController */
            builder.RegisterType<CountCharsFilter>().As<IFilter<string>>();
            builder.RegisterType<UpperCaseFilter>().As<IFilter<string>>();
            builder.RegisterType<LowerCaseFilter>().As<IFilter<string>>();

            /* For LowerValuesController */
            builder.RegisterType<CountCharsFilter>().Keyed<IFilter<string>>(LowerValuesController.Key);
            builder.RegisterType<LowerCaseFilter>().Keyed<IFilter<string>>(LowerValuesController.Key);

            /* For UpperValuesController */
            builder.RegisterType<CountCharsFilter>().Keyed<IFilter<string>>(UpperValuesController.Key);
            builder.RegisterType<UpperCaseFilter>().Keyed<IFilter<string>>(UpperValuesController.Key);

            builder.Populate(services);

            this.ApplicationContainer = builder.Build();

            // Create the IServiceProvider based on the container.
            var rc =  new AutofacServiceProvider(this.ApplicationContainer);
            return rc;
        }

        // Configure is where you add middleware. This is called after
        // ConfigureServices. You can use IApplicationBuilder.ApplicationServices
        // here if you need to resolve things from the container.
        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory, IApplicationLifetime appLifetime)
        {
            loggerFactory.AddConsole(this.Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();

            // If you want to dispose of resources that have been resolved in the
            // application container, register for the "ApplicationStopped" event.
            appLifetime.ApplicationStopped.Register(() => this.ApplicationContainer.Dispose());
        }
    }
}
