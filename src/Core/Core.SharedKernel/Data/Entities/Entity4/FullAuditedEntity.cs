using Core.SharedKernel.Data.Entities.Entity4;

public class FullAuditedEntity : AuditedEntity
{
    public Guid CreatorId { get; set; }

    public Guid UpdaterId { get; set; }

    public FullAuditedEntity(Guid creatorId)
    {
        CreatorId = creatorId;
        UpdaterId = creatorId;
    }
}
