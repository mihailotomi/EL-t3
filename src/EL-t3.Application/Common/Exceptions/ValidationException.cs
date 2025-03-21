using System.Collections;
using FluentValidation.Results;

namespace EL_t3.Application.Common.Exceptions;

public class ValidationException : Exception
{
    private IList<ValidationFailure> ValidationErrors { get; set; }
    public int? StatusCode { get; set; }
    public ValidationException(IList<ValidationFailure> validationErrors, int statusCode = 400)
    {
        ValidationErrors = validationErrors;
        StatusCode = statusCode;
    }

    public ValidationException(string property, string message, int statusCode = 400)
    {
        ValidationErrors = [new ValidationFailure(property, message)];
        StatusCode = statusCode;
    }

    public ValidationException(ValidationFailure validationError, int statusCode = 400)
    {
        ValidationErrors = [validationError];
        StatusCode = statusCode;
    }

    public override IDictionary Data
    {
        get
        {
            var data = new Dictionary<string, IList<string>>();
            foreach (var error in ValidationErrors)
            {
                if (!data.ContainsKey(error.PropertyName))
                {
                    data[error.PropertyName] = new List<string>();
                }

                data[error.PropertyName].Add(error.ErrorMessage);
            }
            return data;
        }
    }

    public override string Message
    {
        get
        {
            return "Request validation failed";
        }
    }
}