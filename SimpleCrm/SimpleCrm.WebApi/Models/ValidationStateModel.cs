using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SimpleCrm.WebApi.Models
{
    public class ValidationStateModel
    {
        public string? Messages { get; set; }
        public List<ValidationError>? Errors { get; set; }

        public ValidationStateModel() { }
        public ValidationStateModel(string error)
        {
            Messages = error;
        }
        public ValidationStateModel(IEnumerable<ValidationError> errors)
        {
            Errors = errors.ToList();
        }

        public ValidationStateModel(ModelStateDictionary modelState)
        {  // modelState has keys for each property that has an associated error
            var genericErrors = modelState.Keys
                .Where(key => string.IsNullOrWhiteSpace(key)) // model only errors have an empty key
                .Select(key => modelState[key].Errors.Select(x => x.ErrorMessage))
                .ToList();

            Messages = genericErrors.Count == 0 ? "Validation failed"
                : string.Join(".", genericErrors.Distinct());

            // now get the property level errors, they have a property name as a key
            Errors = modelState.Keys
                .Where(key => !string.IsNullOrWhiteSpace(key))
                // some Linq magic ...
                .SelectMany(key => modelState[key].Errors
                    .Select(x => new ValidationError { Field = key, Message = x.ErrorMessage }))
                .ToList();
        }
    }
}
