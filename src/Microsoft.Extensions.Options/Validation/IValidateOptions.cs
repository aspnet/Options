namespace Microsoft.Extensions.Options.Validation
{
    /// <summary> 
    /// Represents something that validate the TOptions type. 
    /// </summary>
    /// <typeparam name="TOptions">The type of options being validated.</typeparam> 
    public interface IValidateOptions<in TOptions> where TOptions : class
    {
        /// <summary> 
        /// Invoked to validate a TOptions instance. 
        /// </summary> 
        /// <param name="name">The options name.</param>
        /// <param name="options">The options instance to validate.</param>
        /// <returns><see cref="IValidationResult"/></returns>
        IValidationResult Validate(string name, TOptions options);
    }
}