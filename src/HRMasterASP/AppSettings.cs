using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMasterASP
{
    public class AppSettings
    {
        public IEnumerable<GroupMapping> AADGroups { get; set; }
        public string GroupApplicationId { get; set; }
        public string GroupApplicationKey { get; set; }
        public string AuthenticationContext { get; set; }
        public string UserIdClaimName { get; set; }
    }
    public class GroupMapping
    {
        public string Name { get; set; }
        public string Id { get; set; }
    }
}
