using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMasterASP.Models.CustomValidators
{
    public class NotPastDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return false;
            }

            var date = (DateTime)value;

            if (date < DateTime.Now)
            {
                return false;
            }

            return true;

        }
    }
}

