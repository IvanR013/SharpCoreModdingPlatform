using System.Diagnostics;
using System;
using System.IO;
using System.Threading;
using System.Collections.Generic;
using MSharp.ModAPI;
using MSharp.Launcher.Core.ModRunner;
using MSharp.Launcher.Core.Bridge;

namespace MSharp.Launcher.Core
{
    public class LauncherRunner
    {
        public static void EjecutarMinecraft()
        {
            // Configuración de ejecución de Minecraft
            var javaPath = @"D:\Users\pc\Desktop\Devtools\jdk8u442-b06\bin\java.exe";
            var mainClass = "net.minecraft.launchwrapper.Launch";

            var vmArgs = "-Xmx2G -XX:+UnlockExperimentalVMOptions -XX:+UseG1GC " +
                         "-XX:G1NewSizePercent=20 -XX:G1ReservePercent=20 " +
                         "-XX:MaxGCPauseMillis=50 -XX:G1HeapRegionSize=32M";

            var nativesDir = @"C:\Users\pc\AppData\Roaming\.minecraft\natives";
            var javaArgs = $"-Djava.library.path=\"{nativesDir}\"";

            var classpath = File.ReadAllText(@"D:\Users\pc\Desktop\RoadToM#\MSharp.Launcher.CLI\bin\Debug\net9.0\classpath.txt");

            var forgeArgs = "--username DevTest --version 1.8.9-forge " +
                            "--gameDir \"C:\\Users\\pc\\AppData\\Roaming\\.minecraft\" " +
                            "--assetsDir \"C:\\Users\\pc\\AppData\\Roaming\\.minecraft\\assets\" " +
                            "--assetIndex 1.8 --uuid 1234 --accessToken 1234 " +
                            "--userProperties {} --userType mojang " +
                            "--tweakClass net.minecraftforge.fml.common.launcher.FMLTweaker";

            var launchArgs = $"{vmArgs} {javaArgs} -cp \"{classpath}\" {mainClass} {forgeArgs}";

            var psi = new ProcessStartInfo
            {
                FileName = javaPath,
                Arguments = launchArgs,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = false
            };

            var proceso = new Process { StartInfo = psi };

            proceso.OutputDataReceived += (s, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                    Console.WriteLine($"[OUT] {e.Data}");
            };

            proceso.ErrorDataReceived += (s, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                    Console.WriteLine($"[ERR] {e.Data}");
            };

            // 📦 Cargar mods C# (.dll)
            var modPath = @"D:\Users\pc\Desktop\RoadToM#\Mods#\Mods";
            Console.WriteLine("🔎 Cargando mods C# desde carpeta:");
            Console.WriteLine(modPath);

            var mods = ModLoader.CargarMods(modPath);

            foreach (var mod in mods)
            {
                Console.WriteLine("▶️ Ejecutando OnStart() de mod...");
                mod.OnStart();
            }

            // 🔌 Conexión al mod de Java vía Named Pipe
            var bridge = new NamedPipeBridgeConnection();
            bridge.OnMessage += msg =>
            {
                Console.WriteLine($"📨 Mensaje del mod puente: {msg}");
                foreach (var mod in mods)
                {
                    mod.OnEvent("BRIDGE_MSG", msg);
                }
            };

            Console.WriteLine("🟪 Iniciando servidor de Named Pipe...");
            bridge.Start();

            // 🚀 Lanzar Minecraft
            Console.WriteLine("🚀 Iniciando Minecraft desde C#...");
            proceso.Start();
            proceso.BeginOutputReadLine();
            proceso.BeginErrorReadLine();

            proceso.WaitForExit();
        }
    }
}
