using System.ComponentModel.DataAnnotations;

namespace FakeSpoon.Lib.NostrClient.Events.Values;

public record SubscriptionId : NoteValue
{
    public SubscriptionId(string value)
    {
        Value = value;

        Validate(true);
    }

    public string Value { get; init; }
    
    public sealed override bool Validate(bool throwOnInvalid)
    {
        if (string.IsNullOrWhiteSpace(Value))
        {
            return throwOnInvalid ? throw new ValidationException("Value cannot be null or empty") :false;
        }

        if (Value.Length > 64)
        {
            return throwOnInvalid ? throw new ValidationException("Value cannot be longer than 64 characters") :false;
        }
        
        return true;
    }
}