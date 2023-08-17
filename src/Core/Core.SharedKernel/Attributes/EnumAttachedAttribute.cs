namespace Core.SharedKernel.Attributes;
/// <summary>
/// This Attribute is used for attaching to enum fields. It's inherited.
/// </summary>
[AttributeUsage(AttributeTargets.Field, Inherited = true)]
public class EnumAttachedAttribute : Attribute
{
    /// <summary>
    /// Tag type for styling
    /// </summary>
    public string TagType { get; set; }

    /// <summary>
    /// Chinese description
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Icon
    /// </summary>
    public string Icon { get; set; }

    /// <summary>
    /// Icon color
    /// </summary>
    public string IconColor { get; set; }
}
