using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMasterASP.Models.CustomValidators
{
    public class GreaterThanAttribute : ValidationAttribute
    {
        private decimal _value;

        public GreaterThanAttribute(double value)
        {
            _value = (decimal)value;
        }

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return false;
            }

            var salary = (decimal)value;

            if (salary <= _value)
            {
                return false;
            }

            return true;

        }
    }
}