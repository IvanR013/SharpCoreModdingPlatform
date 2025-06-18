using System.IO.Pipes;
using System.Text;
using System.Text.Json;
using MSharp.Staging.Instruction_adapters;
using MSharp.Launcher.Core.Models;
using MSharp.Validation.Models;

public class FinishAdapterLayer : IInstructionAdapter
{
    private readonly string _pipeName;

    public FinishAdapterLayer(string pipeName = "msharp_bridge") =>  _pipeName = pipeName;
    

    public InstructionValidationResult Validate(MSharpInstruction instruction)
    {
        var response = SendPayloadOverPipe(instruction, isValidation: true);

        return response?.ToLowerInvariant() switch
        {
            "ok" => InstructionValidationResult.Success(),
            var msg => InstructionValidationResult.Failure(msg ?? "Respuesta nula del adaptador.")
        };
    }

    public bool Apply(MSharpInstruction instruction)
    {
        var response = SendPayloadOverPipe(instruction, isValidation: false);
        return response?.ToLowerInvariant() == "ok";
    }

    private string? SendPayloadOverPipe(MSharpInstruction instruction, bool isValidation)
    {
        try
        {
            using var client = new NamedPipeClientStream(".", _pipeName, PipeDirection.InOut, PipeOptions.None);
            client.Connect(2000); // Timeout de 2s

            var payloadJson = JsonSerializer.Serialize(new
            {
                type = isValidation ? "validate" : "apply",
                data = instruction
            });

            byte[] buffer = Encoding.UTF8.GetBytes(payloadJson);
            client.Write(buffer, 0, buffer.Length);
            client.Flush();

            // Leer respuesta
            byte[] responseBuffer = new byte[256];
            int bytesRead = client.Read(responseBuffer, 0, responseBuffer.Length);
            return Encoding.UTF8.GetString(responseBuffer, 0, bytesRead);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error en SendPayloadOverPipe: {ex.Message}");
            return null;
        }
    }
}
