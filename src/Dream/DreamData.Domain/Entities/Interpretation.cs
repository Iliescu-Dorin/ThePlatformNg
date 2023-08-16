using Core.SharedKernel.Entities;
using Core.SharedKernel.Records;
using DreamData.Domain.Enums;

namespace DreamData.Domain.Entities;
public class Interpretation : Entity<Guid>
{
    public Interpretation(Guid id) : base(id)
    {

    }
    public string ExtractedText { get; set; } = string.Empty;
    public InterpretationSelectedText SelectedText { get; set; } = new InterpretationSelectedText("0", "0");
    public string Meaning { get; set; } = string.Empty;
    public CultureBeliefs Culture { get; set; }
}
