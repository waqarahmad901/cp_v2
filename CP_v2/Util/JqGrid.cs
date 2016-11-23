using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CP_v2.Util
{

    public class Row
    {
        public string id { get; set; }
        public List<string> cell { get; set; }
    }

    public class JqGrid
    {
        public int page { get; set; }
        public int total { get; set; }
        public int records { get; set; }
        public List<Row> rows { get; set; }
    }

}