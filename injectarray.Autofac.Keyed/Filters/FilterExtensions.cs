using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace injectarray.Filters
{
    public static class FilterExtensions
    {
        public static T All<T>(this IEnumerable<IFilter<T>> filters, ILogger logger, T t)
        {
            var rc = t;
            foreach (var filter in filters)
            {
                logger.LogDebug($"Before: value: {rc}, filter: {filter.GetType().Name}");
                rc = filter.Filter(rc);
                logger.LogDebug($"After: value: {rc}, filter: {filter.GetType().Name}");                
            }
            return rc;
        }
    }
}
