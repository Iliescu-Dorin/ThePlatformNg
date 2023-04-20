using Core.SharedKernel.Entities;
using DreamDomain.Enums;

namespace DreamDomain.Entities;
public class Interpretation : Entity<Guid>
{
    public Interpretation(Guid id) : base(id)
    {
    }

    public Guid Id { get; set; }
    public string ExtractedText { get; set; } = string.Empty;
    public InterpretationSelectedText SelectedText { get; set; }
    public string Meaning { get; set; } = string.Empty;
    public CultureBeliefs Culture { get; set; }
}
public record InterpretationSelectedText(string StartOffset, string EndOffset);