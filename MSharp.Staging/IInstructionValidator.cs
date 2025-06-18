using MSharp.Validation.Models;
namespace MSharp.Validation;

public interface IInstructionValidator
{
	InstructionValidationResult Validate(string json);
}