namespace User.Domain.ValueObjects;
public class Name
{
    public string FirstName { get; }
    public string LastName { get; }
    public Name(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }
}
