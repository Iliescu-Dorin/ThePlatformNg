using Core.SharedKernel.Data.ValueObjects;

namespace Core.SharedKernel.DTO;

public class HealthCheckDTO
{
    public string OverallStatus { get; set; }
    public IEnumerable<HealthCheckRecord> HealthChecks { get; set; }
    public string TotalDuration { get; set; }
}
