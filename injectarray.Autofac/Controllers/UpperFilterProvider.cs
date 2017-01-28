using System.Collections.Generic;
using injectarray.Filters;

namespace injectarray.Controllers
{
    public class UpperFilterProvider : IFilterProvider<string>
    {
        public IEnumerable<IFilter<string>> Filters { get; }

        public UpperFilterProvider(UpperCaseFilter upperCaseFilter, CountCharsFilter countCharsFilter)
        {
            Filters = new IFilter<string>[]
            {
                upperCaseFilter,
                countCharsFilter
            };
        }
    }
}