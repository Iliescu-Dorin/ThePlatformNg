using Core.SharedKernel.Data.ValueObjects;

public class HealthCheck : ValueObject
{
    public string Status { get; private set; }
    public string Component { get; private set; }
    public string Description { get; private set; }
    public string Duration { get; private set; }

    // Private constructor to enforce object creation through static methods
    private HealthCheck(string status, string component, string description, string duration)
    {
        Status = status;
        Component = component;
        Description = description;
        Duration = duration;
    }

    public static HealthCheck Create(string status, string component, string description, string duration)
    {
        // You can add validation logic here before creating the object
        return new HealthCheck(status, component, description, duration);
    }

    // Implement the GetEqualityComponents method to define equality comparison
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Status;
        yield return Component;
        yield return Description;
        yield return Duration;
    }
}
