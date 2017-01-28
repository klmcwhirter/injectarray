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
            ** The .AsSelf() extension method is nice - we avoid registration duplication with it
            **
            ** Remember that the ValuesController wants IEnumerable<IFilter<string>> and
            ** the LowerFilterProvider and UpperFilterProvider want the concrete types such as
            ** CountCharsFilter, etc.
            */

            // Add app filters            
            builder.RegisterType<CountCharsFilter>().AsSelf().As<IFilter<string>>();
            builder.RegisterType<LowerCaseFilter>().AsSelf().As<IFilter<string>>();
            builder.RegisterType<UpperCaseFilter>().AsSelf().As<IFilter<string>>();

            builder.RegisterType<LowerFilterProvider>();
            builder.RegisterType<UpperFilterProvider>();

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
