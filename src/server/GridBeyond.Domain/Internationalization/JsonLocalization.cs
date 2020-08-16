using System.Collections.Generic;

namespace GridBeyond.Domain.Internationalization
{
    internal class JsonLocalization
    {
        public string Key { get; set; }
        public Dictionary<string,string> LocalizedValues { get; set; }
    }
}