using MSharp.ModLoader.StagingSystem;
using MSharp.Validation.Models;
using MSharp.Staging.Instruction_adapters;
using MSharp.Validation;
using MSharp.Launcher.Core.Models;

namespace MSharp.Staging;

public class InstructionProcessor
{
	private readonly IInstructionValidator _validator;
	private readonly IInstructionAdapter _adapter;
	private readonly StagingManager<MSharpInstruction> _stager;

	public InstructionProcessor(IInstructionValidator validator, IInstructionAdapter adapter, StagingManager<MSharpInstruction> stager)
	{
		_validator = validator;
		_adapter = adapter;
		_stager = stager;
	}

	public InstructionValidationResult Process(string json)
	{
		try
		{
			InstructionValidationResult result = _validator.Validate(json);
			if (!result.IsValid) return result;

			var instruction = JsonSerializer.Deserialize<MSharpInstruction>(json);

			if (instruction is not null)
			{
				_stager.MSadd(instruction);

				if (_adapter.Apply(instruction))
				{
					_stager.MScommit();
					return InstructionValidationResult.Success();
				}
			}
			_stager.MSrevert();
			return InstructionValidationResult.Failure("Adapter rejected the instruction");
		}
		catch
		{
   			return InstructionValidationResult.Failure("An error occurred while processing the instruction");
		}
	}
}
