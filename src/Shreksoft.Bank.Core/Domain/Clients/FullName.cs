namespace Shreksoft.Bank.Core.Domain.Clients;

/// <summary>
/// Represents a client's full name as a value object.
/// </summary>
/// <remarks>
/// In this decision, FullName was implemented as struct. However, this causes an issue with initialization because structs in C# can bypass the constructor, potentially leaving fields null
/// </remarks>
public readonly record struct FullName
{
    private const string IncorrectInputDataMessage = "Can't be null or white space";
    public string FirstName { get; }
    public string? MiddleName { get; }
    public string LastName { get; }

    public FullName(string firstName, string? middleName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException(IncorrectInputDataMessage, nameof(firstName));

        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException(IncorrectInputDataMessage, nameof(lastName));

        FirstName = ToTitleCase(firstName);
        MiddleName = string.IsNullOrWhiteSpace(middleName) ? null : ToTitleCase(middleName);
        LastName = ToTitleCase(lastName);
    }

    private static string ToTitleCase(string str)
    {
        var chars = str.Trim().ToArray();
        chars[0] = char.ToUpperInvariant(chars[0]);

        for (var i = 1; i < chars.Length; i++) chars[i] = char.ToLowerInvariant(chars[i]);

        return new string(chars);
    }

    public override string ToString()
    {
        return MiddleName is null ? $"{FirstName} {LastName}" : $"{FirstName} {MiddleName} {LastName}";
    }
}
