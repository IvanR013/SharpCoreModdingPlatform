using MSharp.Validation.Models;
using MSharp.Launcher.Core.Models;

namespace MSharp.Staging.Instruction_adapters;

public interface IInstructionAdapter
{
	InstructionValidationResult Validate(MSharpInstruction instruction);
	bool Apply(MSharpInstruction instruction);
}
