namespace MSharp.Validation.Models;
public class InstructionValidationResult
{
    public bool IsValid { get; }
    public string? ErrorMessage { get; }

    private InstructionValidationResult(bool isValid, string? errorMessage)
    {
        IsValid = isValid;
        ErrorMessage = errorMessage;
    }

    public static InstructionValidationResult Success() => new(true, null);
    public static InstructionValidationResult Failure(string message) => new(false, message);
}
