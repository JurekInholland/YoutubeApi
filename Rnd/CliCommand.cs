using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Rnd;

public static class CliCommand
{
    private static readonly string[] Shell = GetCommandShell();


    private static string[] GetCommandShell()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX) || RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            return new[] {"/bin/bash", "-c"};
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return new[] {"cmd.exe", "/c"};
        }

        throw new NotSupportedException("Unsupported platform.");
    }

    public static async Task<string> CallCommandWithReturn(string cmd)
    {
        var sw = new Stopwatch();
        sw.Start();
        cmd = cmd.Replace("\"", "\\\"");
        using var process = new Process();
        Console.WriteLine(sw.ElapsedMilliseconds);
        process.StartInfo.FileName = Shell[0];
        process.StartInfo.Arguments = $"{Shell[1]} \"{cmd}\"";
        process.StartInfo.UseShellExecute = false;

        process.StartInfo.RedirectStandardOutput = true;
        process.Start();
        Console.WriteLine(sw.ElapsedMilliseconds);
        var res = await process.StandardOutput.ReadToEndAsync();
        Console.WriteLine(sw.ElapsedMilliseconds);
        await process.WaitForExitAsync();
        Console.WriteLine(sw.ElapsedMilliseconds);
        sw.Stop();
        return res;
    }

    /// <summary>
    /// Execute a shell command and return the output
    /// </summary>
    public static async Task CallCommand(string cmd, DataReceivedEventHandler callback = null!)
    {
        using var process = new Process();
        process.StartInfo.FileName = Shell[0];
        var args =  $@"{Shell[1]} ""{cmd}""";
        Console.WriteLine("CallCommand args: " + args);
        process.StartInfo.Arguments = args;

        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;

        process.OutputDataReceived += callback;

        process.Start();
        process.BeginOutputReadLine();

        await process.WaitForExitAsync();

        if (process.ExitCode != 0)
        {
            throw new CliException($"Command failed with exit code {process.ExitCode} - {await process.StandardError.ReadToEndAsync()}");
        }
    }
}
