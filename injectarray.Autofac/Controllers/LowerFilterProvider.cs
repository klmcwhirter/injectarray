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