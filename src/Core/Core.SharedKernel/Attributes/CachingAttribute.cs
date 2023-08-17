namespace Core.SharedKernel.Attributes;

/// <summary>
/// This Attribute is used for validation during usage. Add it to the methods where you want to cache data to perform caching operations.
/// </summary>
[AttributeUsage(AttributeTargets.Method, Inherited = true)]
public class CachingAttribute : Attribute
{
    /// <summary>
    /// Absolute cache expiration time (in minutes)
    /// </summary>
    public int AbsoluteExpiration { get; set; } = 30;
}
