using System.IO.Pipes;
using System.Text;
using System.Text.Json;
using MSharp.Launcher.dummyInstructions;
using MSharp.ModLoader.StagingSystem;

namespace MSharp.Launcher.Core.Bridge
{
    public class NamedPipeBridgeConnection : IBridgeConnection
    {
        private readonly string pipeName;
        private NamedPipeServerStream? server;
        private Thread? listenThread;

        private readonly StagingManager _stageManager = new(); // Manejador de staging para aplicar y revertir instrucciones

        public event Action<string>? OnMessage; // Esto queda por compatibilidad, pero ya no es el punto de entrada principal

        public NamedPipeBridgeConnection(string pipeName = "msharp_bridge")
        {
            this.pipeName = pipeName;
        }

        public void Start()
        {
            listenThread = new Thread(() =>
            {
                while (true)
                {
                    try
                    {
                        server = new NamedPipeServerStream(
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

                        Console.WriteLine("📡 Pipe levantado. Esperando conexión Java...");
                        server.WaitForConnection();
                        Console.WriteLine("🟣 ¡Conexión Java ↔ C# establecida!");

                        byte[] buffer = new byte[2048];
                        while (server.IsConnected)
                        {
                            int bytesRead = server.Read(buffer, 0, buffer.Length);
                            if (bytesRead == 0) break;

                            string json = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                            HandleIncomingInstruction(json);
                        }
                    }
                    catch (IOException ioEx)
                    {
                        Console.WriteLine($"⚠️ I/O Pipe error: {ioEx.Message}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"❌ Error general: {ex.Message}");
                    }
                    finally
                    {
                        server?.Dispose();
                        server = null;
                        Console.WriteLine("🔁 Pipe cerrado. Esperando nueva conexión...");
                        Thread.Sleep(1000);
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
                if (server is { IsConnected: true })
                {
                    byte[] buffer = Encoding.UTF8.GetBytes(message);
                    server.Write(buffer, 0, buffer.Length);
                    server.Flush();
                }
                else
                {
                    Console.WriteLine("⚠️ No se puede enviar mensaje. No hay conexión.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al enviar por pipe: {ex.Message}");
            }
        }

        private void HandleIncomingInstruction(string json)
        {
            try
            {
                var payload = JsonSerializer.Deserialize<MSharpInstruction>(json);
                if (payload == null)
                {
                    Console.WriteLine("⚠️ Instrucción vacía o inválida.");
                    return;
                }

                _stageManager.Stage(payload);

                // Ejecutamos el adapter Java o cualquier otro
                bool success = ExecuteInstruction(payload);

                if (success)
                {
                    _stageManager.Commit(payload.Entidad);
                    Console.WriteLine("✅ Instrucción aplicada y comiteada.");
                }
                else
                {
                    _stageManager.Rollback(payload.Entidad);
                    Console.WriteLine("🔄 Instrucción fallida. Se hizo rollback.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error procesando instrucción: {ex.Message}");
            }
        }

        private bool ExecuteInstruction(MSharpInstruction payload)
        {
            try
            {
                // 🔌 Este sería el "adapter" real → Lo podés redirigir al mod de Java, o a cualquier handler
                Console.WriteLine($"▶️ Ejecutando {payload.Tipo} para entidad {payload.Entidad}");

                // Este método deberías adaptarlo a tu arquitectura real
                // Simulación por ahora
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
