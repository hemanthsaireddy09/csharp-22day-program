using System.ComponentModel.DataAnnotations;

//value is DateTime date: It safely checks if the user actually typed a valid date. If yes, it saves that input into a temporary variable named date.
//date.Date < DateTime.Today: It strips out any time details (setting the clock to midnight) and checks if the selected day comes before today's current date.
//return new ValidationResult(...): If the date is in the past, it stops the request right there and sends back your custom error message to display on the screen.

using System.ComponentModel.DataAnnotations;

namespace EHRMvcAuditLedgerDemo.Validation
{
    public class FutureOrTodayDateAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext context)
        {
            if (value is DateTime date && date.Date < DateTime.Today)
                return new ValidationResult("Transaction date cannot be in the past");

            return ValidationResult.Success;
        }
    }
}