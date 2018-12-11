using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HRMasterASP.Models
{
    public class JobApplicationCreateView : JobApplication
    {
        public string JobOfferName { get; set; }

        [Required]
        [Display(Name = "CV File")]
        [FileExtensions(Extensions = (".pdf,.doc,.docx"), ErrorMessage = "Please select a pdf or a Word file.")]
        public IFormFile CvFile { get; set; }
    }
}
