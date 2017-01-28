namespace injectarray.Filters
{
    public class LowerCaseFilter : IFilter<string>
    {
        public string Filter(string arg)
        {
            var rc = arg.ToLowerInvariant();
            return rc;
        }
    }
}