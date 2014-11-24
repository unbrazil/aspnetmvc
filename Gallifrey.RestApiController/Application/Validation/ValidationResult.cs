using System.Collections.Generic;

namespace Gallifrey.RestApi.Application.Validation
{
    public class ValidationResult
    {
        public bool IsSuccess { get; set; }

        public IList<string> Messages { get; set; }
    }
}