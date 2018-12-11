using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace HRMasterASP.Models
{
    public class JobApplication
    {
        public int Id { get; set; }

        [Required]
        public int OfferId { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Contact Email Address")]
        public string EmailAddress { get; set; }

        [Required]
        [Display(Name = "I agree to contact for recruitment purposes")]
        public bool ContactAgreement { get; set; }

        [Display(Name = "CV URL")]
        public string CvUrl { get; set; }

        [Required]
        [Display(Name = "Date Of Birth")]
        public DateTime DateOfBirth { get; set; }

        [MinLength(100)]
        public string Description { get; set; }
    }
}
