namespace Microsoft.Extensions.Options.Validation
{
    /// <summary> 
    /// Used to specify the validation status for <see cref="IValidationResult"/>. 
    /// </summary> 
    public enum ValidationStatus
    {
        /// <summary> 
        /// Says that critical validation is failed. 
        /// </summary> 
        Invalid = 0,

        /// <summary> 
        /// Says that validation is passed but it's fine. 
        /// </summary> 
        Valid = 1,

        /// <summary> 
        /// Says that validation is failed but it's fine. 
        /// </summary> 
        Warning = 2
    }
}