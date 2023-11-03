using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SimpleCrm.WebApi.Models;

namespace SimpleCrm.WebApi.Filters
{
    public class ValidationFailedResult : ObjectResult
    {
        // just override the constructor, call the base constructor
        public ValidationFailedResult(ModelStateDictionary modelState)
            : base(new ValidationStateModel(modelState))
        {
            StatusCode = StatusCodes.Status422UnprocessableEntity;
        }
    }
}
