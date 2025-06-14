namespace MSharp.Launcher.Core.Models
{
    public class MSharpInstruction
    {
        public string? Tipo { get; set; }              
        public string? Entidad { get; set; }           
        public Dictionary<string, object>? Datos { get; set; }   // Payload flexible
    }
}
