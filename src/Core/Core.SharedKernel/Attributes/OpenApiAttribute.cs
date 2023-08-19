namespace Core.SharedKernel.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
public class OpenApiAttribute : Attribute
{
}
