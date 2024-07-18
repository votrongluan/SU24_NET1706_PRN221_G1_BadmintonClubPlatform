using System.ComponentModel.DataAnnotations;

namespace BusinessObjects.Validations;

public class TimeRangeAttribute : ValidationAttribute
{
    private readonly TimeOnly _minTime;
    private readonly TimeOnly _maxTime;

    public TimeRangeAttribute(string minTime, string maxTime)
    {
        _minTime = TimeOnly.Parse(minTime);
        _maxTime = TimeOnly.Parse(maxTime);
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is TimeOnly timeValue)
        {
            if (timeValue < _minTime || timeValue > _maxTime)
            {
                return new ValidationResult(ErrorMessage ?? $"Time must be between {_minTime} and {_maxTime}.");
            }
        }
        return ValidationResult.Success;
    }
}