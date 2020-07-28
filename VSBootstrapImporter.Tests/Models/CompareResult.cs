using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSBootstrapImporter.Tests.Models
{
    public class CompareResult
    {
        public CompareResult()
        {
            CountDifferent = true;
            ContentsDifferent = true;
            File1Count = 0;
            File2Count = 0;
        }
        public bool CountDifferent { get; set; }
        public bool ContentsDifferent { get; set; }
        public int File1Count { get; set; }
        public int File2Count { get; set; }
    }
}
