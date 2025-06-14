namespace MSharp.Launcher.Core.Models
{
    public class MSharpInstruction
    {
        public string? Tipo { get; set; }                // Ej: "AddMod", "RemoveMod", etc.
        public string? Entidad { get; set; }             // Identificador de la entidad/mod
        public Dictionary<string, object>? Datos { get; set; }   // Payload flexible
    }
}
