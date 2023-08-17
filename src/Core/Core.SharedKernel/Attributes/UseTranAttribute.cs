namespace Core.SharedKernel.Attributes;

/// <summary>
/// This Attribute is used for validation during usage. Add it to the methods where you want to perform transactions to execute transactional operations.
/// </summary>
[AttributeUsage(AttributeTargets.Method, Inherited = true)]
public class UseTranAttribute : Attribute
{
    /// <summary>
    /// Transaction propagation mode
    /// </summary>
    public Propagation Propagation { get; set; } = Propagation.Required;
}
