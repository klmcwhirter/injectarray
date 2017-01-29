namespace injectarray.Filters
{
    public class UpperCaseFilter : IFilter<string>
    {
        public string Filter(string arg)
        {
            var rc = arg.ToUpperInvariant();
            return rc;
        }
    }
}