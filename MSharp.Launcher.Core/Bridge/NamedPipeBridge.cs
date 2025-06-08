using System.IO.Pipes;
using System.Text;


namespace MSharp.Launcher.Core.Bridge
{
    public class NamedPipeBridgeConnection : IBridgeConnection
    {
        private readonly string pipeName;
        private NamedPipeServerStream? server;
        private Thread? listenThread;

        public event Action<string>? OnMessage;

        public NamedPipeBridgeConnection(string pipeName = "msharp_bridge")
        {
            this.pipeName = pipeName;
        }

        public void Start()
        {
            listenThread = new Thread(() =>
            {
                for(; ;) // Loop infinito al estilo programador de cpp pro para esperar reconexiones
                {
                    try
                    {
                        this.server = new NamedPipeServerStream(
                            pipeName,
                            PipeDirection.InOut,
                            1,
                    #if WINDOWS
                            PipeTransmissionMode.Message,
                    #else
                            PipeTransmissionMode.Byte,
                    #endif
                            PipeOptions.Asynchronous
                        );

                        Console.WriteLine("📡 Servidor de pipe levantado en modo: Message");
                        Console.WriteLine("🟪 Esperando conexión desde el mod Java...");
                        server.WaitForConnection();
                        Console.WriteLine("🟣 ¡Conexión Java ↔ C# establecida!");

                        byte[] buffer = new byte[1024];
                        while (server.IsConnected)
                        {
                            int bytesRead = server.Read(buffer, 0, buffer.Length);
                            if (bytesRead == 0)
                            {
                                Console.WriteLine("🔌 Cliente Java desconectado.");
                                break;
                            }

                            string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                            OnMessage?.Invoke(message);
                        }
                    }
                    catch (IOException ioEx)
                    {
                        Console.WriteLine($"⚠️ Error I/O del pipe: {ioEx.Message}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"❌ Error general en NamedPipeBridgeConnection: {ex.Message}");
                    }
                    finally
                    {
                        server?.Dispose();
                        server = null;
                        Console.WriteLine("♻️ Pipe cerrado y limpio. Esperando nueva conexión...");
                        Thread.Sleep(1000); // Evitá que el loop queme CPU en reconexión
                    }
                }
            })
            {
                IsBackground = true,
                Name = "MSharpNamedPipeListener"
            };

            listenThread.Start();
        }



        public void Send(string message)
        {
            try
            {
                if (server is { IsConnected: false })
                {
                    byte[] buffer = Encoding.UTF8.GetBytes(message);
                    server.Write(buffer, 0, buffer.Length);
                    server.Flush();
                }
                else
                {
                    Console.WriteLine("⚠️ No se puede enviar mensaje. No hay conexión activa.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error enviando mensaje al pipe: {ex.Message}");
            }
        }
    }
}
