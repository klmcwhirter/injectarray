using System.Collections.Generic;
using Autofac.Features.Indexed;
using injectarray.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace injectarray.Controllers
{
    [Route("api/[controller]")]
    public class LowerValuesController : Controller
    {
        public const string Key = "lower";
        IEnumerable<IFilter<string>> Filters { get; }

        ILogger Logger { get; }
        
        public LowerValuesController(IIndex<string,IEnumerable<IFilter<string>>> filters, ILogger<ValuesController> logger)
        {
            Filters = filters[LowerValuesController.Key]; // This is the Autofac keyed injected type
            Logger = logger;

            Logger.LogDebug($"{GetType().Name}.Ctor");
        }

        static readonly string[] values = {
            "This is a value",
            "this is another value"
        };

        // GET api/values
        [HttpGet]
        public IEnumerable<Values> Get()
        {
            Logger.LogDebug($"{GetType().Name}.Get entering");
            
            var rc = new List<Values>();
            foreach (var valu in values)
            {
                var newValue = Filters.All(Logger, valu);
                rc.Add(new Values { OldValue = valu, NewValue = newValue });
            }

            Logger.LogDebug($"{GetType().Name}.Get exiting");
            
            return rc;
        }
    }
}
