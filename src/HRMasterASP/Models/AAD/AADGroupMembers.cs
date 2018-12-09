using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMasterASP.Models.AAD
{
    public class AADGroupMembers
    {
        public List<Value> value { get; set; }
    }
    public class Value
    {
        public string url { get; set; }
    }
}
