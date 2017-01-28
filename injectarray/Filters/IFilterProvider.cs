using System.Collections.Generic;

namespace injectarray.Filters
{
    public interface IFilterProvider<T>
    {
        IEnumerable<IFilter<T>> Filters { get; }
    }
}
