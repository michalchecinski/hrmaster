using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMasterASP.Models
{
    public class JobApplication
    {
        public int Id { get; set; }
        public int OfferId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public bool ContactAgreement { get; set; }
        public string CvUrl { get; set; }
    }
}
