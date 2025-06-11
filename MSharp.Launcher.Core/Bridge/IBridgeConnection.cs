namespace MSharp.Launcher.Core.Bridge;

public interface IBridgeConnection // Contrato para la definición de conexiones entre C# y Java
{
    void Start();
    void Send(string message);
    event Action<string> OnMessage;
}
