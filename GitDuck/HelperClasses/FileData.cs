using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GitDuck.HelperClasses
{
    public class FileData
    {
        public string name { get; set; }
        public string path { get; set; }
        public int size { get; set; }
        public string type { get; set; }
        public string content { get; set; }
    }
}
