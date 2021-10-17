using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Book_Store_App.Helpers
{
    public class MaxLengthValidation : ValidationAttribute
    {
        public int MaxLength { get; set; }
        public MaxLengthValidation(int maxLength)
        {
            MaxLength = maxLength;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if(value != null)
            {
                string bookTitle = value.ToString();

                if (bookTitle.Length <= MaxLength)
                {
                    return ValidationResult.Success;
                }
                return new ValidationResult(ErrorMessage ?? $"Max langth for book title is {MaxLength} letters");
            }

            return new ValidationResult("Input is empty");
        }
    }
}
