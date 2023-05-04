using Core.SharedKernel.Entities;
using DreamDomain.Enums;

namespace DreamDomain.Entities;
public class Interpretation : Entity<Guid>
{
    public Interpretation(Guid id) : base(id)
    {

    }

    public new Guid Id { get; set; }
    public string ExtractedText { get; set; } = string.Empty;
    public InterpretationSelectedText SelectedText { get; set; } = new InterpretationSelectedText("0", "0");
    public string Meaning { get; set; } = string.Empty;
    public CultureBeliefs Culture { get; set; }
}
public record InterpretationSelectedText(string StartOffset, string EndOffset);
