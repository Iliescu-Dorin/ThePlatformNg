namespace Core.SharedKernel.DTO;
public class CreateOrUpdateDreamDTO
{
    public Guid UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime Date { get; set; } = DateTime.UtcNow;
    public List<string>? Symbols { get; set; } = new List<string>();
    public List<InterpretationDTO> Interpretations { get; set; } = new List<InterpretationDTO>();
}
