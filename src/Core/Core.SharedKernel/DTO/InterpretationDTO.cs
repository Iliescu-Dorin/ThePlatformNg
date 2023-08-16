using Core.SharedKernel.Records;
using DreamData.Domain.Enums;

namespace Core.SharedKernel.DTO;
public class InterpretationDTO
{
    public string ExtractedText { get; set; } = string.Empty;
    public InterpretationSelectedText SelectedText { get; set; } = new InterpretationSelectedText("0", "0");
    public string Meaning { get; set; } = string.Empty;
    public CultureBeliefs Culture { get; set; }
}
