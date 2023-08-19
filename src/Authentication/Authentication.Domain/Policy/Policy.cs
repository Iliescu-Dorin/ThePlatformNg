using Core.SharedKernel.Data.Entities.Entity4;

namespace Authentication.Domain.Policy;

public class Policy : AuditedEntity
{
    public Guid? OrganizationId { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public string Type { get; set; }

    public ICollection<PolicyStatement> Statements { get; set; }

    public Policy(Guid organizationId, string name, string description)
    {
        OrganizationId = organizationId;
        Name = name;
        Description = description;

        Type = PolicyTypes.CustomerManaged;
        Statements = Array.Empty<PolicyStatement>();
    }

    public void UpdateSetting(string name, string description)
    {
        Name = name;
        Description = description;

        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateStatements(ICollection<PolicyStatement> statements)
    {
        Statements = statements ?? Array.Empty<PolicyStatement>();

        UpdatedAt = DateTime.UtcNow;
    }
}
