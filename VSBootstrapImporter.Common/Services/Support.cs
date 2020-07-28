using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
