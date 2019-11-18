using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;


namespace Application.Items.Models.Validation
{
    public class Status : ValidationAttribute
    {
        private readonly List<string> _statuses;

        public Status(string status, string status2)
        {
            _statuses = new List<string>() { status, status2 };
        }

       protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var isValid = false;

            if(value is null)
            {
                return ValidationResult.Success;
            }

            var status = (string)value;

            foreach(string s in _statuses)
            {
                isValid = status.Trim().ToLower() == s.Trim().ToLower() ? true : isValid;
            }
            if (isValid)
            {
                return ValidationResult.Success;
            }
            return new ValidationResult(ErrorMessage);
        }
    }
}
