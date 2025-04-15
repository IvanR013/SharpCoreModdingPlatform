using System.Diagnostics;

namespace MinecraftLauncherCORE
{
    public class MinecraftLauncher
    {
        public void LaunchVanilla(string javaPath, string gameJar, string mainClass, string args, string workingDirectory)
        {
            var psi = new ProcessStartInfo
            {
                FileName = javaPath, // Ruta a java.exe
                Arguments = $"-cp \"{gameJar}\" {mainClass} {args}",
                WorkingDirectory = workingDirectory,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
            };

            var process = new Process
            {
                StartInfo = psi
            };

            process.OutputDataReceived += (s, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                    Console.WriteLine($"[MINECRAFT STDOUT] {e.Data}");
            };

            process.ErrorDataReceived += (s, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                    Console.WriteLine($"[MINECRAFT STDERR] {e.Data}");
            };

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
        }
    }
}