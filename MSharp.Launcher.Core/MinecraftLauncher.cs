using System.Diagnostics;

namespace MinecraftLauncherCORE
{
    public class MinecraftLauncher
    {
        public void LaunchVanilla(string javaPath, string gameJar, string mainClass, string args, string workingDirectory)
        {
            ProcessStartInfo psi = new()
            {
                FileName = javaPath, // Ruta a java.exe
                Arguments = $"-cp \"{gameJar}\" {mainClass} {args}",
                WorkingDirectory = workingDirectory,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
            };

            Process prcss = new()
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

            prcss.Start();
            prcss.BeginOutputReadLine();
            prcss.BeginErrorReadLine();
        }
    }
}