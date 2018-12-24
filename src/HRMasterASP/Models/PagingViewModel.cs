using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMasterASP.Models
{
    public class PagingViewModel<T> where T: class
    {
        public IEnumerable<T> Set { get; set; }
        public int TotalPage { get; set; }
    }
}
