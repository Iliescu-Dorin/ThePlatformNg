namespace Core.SharedKernel.DTO;
public class ResultDTO
{
    public bool Succeeded { get; set; }
    public string[] Errors { get; set; }

    internal ResultDTO(bool succeeded, IEnumerable<string> errors)
    {
        Succeeded = succeeded;
        Errors = errors.ToArray();
    }

    public static ResultDTO Success()
    {
        return new ResultDTO(true, System.Array.Empty<string>());
    }

    public static ResultDTO Failure(IEnumerable<string> errors)
    {
        return new ResultDTO(false, errors);
    }
}
