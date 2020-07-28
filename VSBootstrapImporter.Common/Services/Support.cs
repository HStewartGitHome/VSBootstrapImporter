namespace VSBootstrapImporter.Common.Services
{
    public static class Support
    {
        public static string Quote(string str)
        {
            string result = '"' + str + '"';
            return result;
        }
    }
}
