namespace Core.SharedKernel.Data.ValueObjects;
public record HealthCheckRecord(string Status, string Component, string Description, string Duration);
