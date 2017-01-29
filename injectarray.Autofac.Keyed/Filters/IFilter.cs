namespace injectarray.Filters
{
    public interface IFilter<T>
    {
        T Filter(T arg);
    }
}
