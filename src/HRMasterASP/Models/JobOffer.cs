using HRMasterASP.Models;
using HRMasterASP.Models.CustomValidators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HRMasterASP.Models
{
    public class JobOffer : IValidatableObject
    {
        public int Id { get; set; }

        [Display(Name = "Job title")]
        [Required]
        public string JobTitle { get; set; }

        public virtual Company Company { get; set; }
        public virtual int CompanyId { get; set; }

        [Display(Name = "Salary from")]
        [GreaterThan(0, ErrorMessage ="Salary from must be greater than 0")]
        public decimal? SalaryFrom { get; set; }

        [Display(Name = "Salary to")]
        [GreaterThan(0, ErrorMessage = "Salary to must be greater than 0")]
        public decimal? SalaryTo { get; set; }

        public DateTime Created { get; set; }

        public string Location { get; set; }

        [Required]
        [MinLength(100)]
        public string Description { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode =true, DataFormatString ="{0:yyy-MM-dd}")]
        [Display(Name ="Valid until")]
        [NotPastDate(ErrorMessage = "Valid until cannot be past date")]
        public DateTime? ValidUntil { get; set; }

        public List<JobApplication> JobApplications { get; set; } = new List<JobApplication>();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (SalaryFrom > SalaryTo)
            {
                yield return new ValidationResult(
                    $"Salary To must be grater than Salary From", new[] { "SalaryFrom", "SalaryTo" });
            }
        }

    }
}
