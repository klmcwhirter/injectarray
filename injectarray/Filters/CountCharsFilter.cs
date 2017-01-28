namespace injectarray.Filters
{
    public class CountCharsFilter : IFilter<string>
    {
        public string Filter(string arg)
        {
            var rc = $"{arg} {arg.Length}";
            return rc;
        }
    }
}