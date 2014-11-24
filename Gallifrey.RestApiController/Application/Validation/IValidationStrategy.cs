namespace Gallifrey.RestApi.Application.Validation
{
    public interface IValidationStrategy<in T> : IValidationStrategy
    {
        ValidationResult Validate(T val);
    }

    public interface IValidationStrategy
    {
        ValidationResult Validate(object val);
    }
}