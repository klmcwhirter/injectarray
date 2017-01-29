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

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();

            // Add app filters

            /* ValuesController uses these */
            services.AddTransient<IFilter<string>, CountCharsFilter>();
            services.AddTransient<IFilter<string>, UpperCaseFilter>();
            services.AddTransient<IFilter<string>, LowerCaseFilter>();

            /* LowerValuesController and UpperValuesController use these */
            services.AddTransient<CountCharsFilter>();
            services.AddTransient<UpperCaseFilter>();
            services.AddTransient<LowerCaseFilter>();

            services.AddTransient<LowerFilterProvider>();
            services.AddTransient<UpperFilterProvider>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();
        }
    }
}
