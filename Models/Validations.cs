using System;
using System.ComponentModel.DataAnnotations;

namespace Validations 
{
    public class FutureDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime dateTime = new DateTime();
            if (value is DateTime)
                dateTime = (DateTime)value;
            if (DateTime.Compare(dateTime, DateTime.Now) < 0)
                return new ValidationResult("that wedding already happened bro");
            return ValidationResult.Success;
        }
    }
}
