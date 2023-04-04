using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System;

namespace Rnd;

public class CliCommand
{
    private static string[] shell = GetCommandShell();

    private static Stream? _outputStream;

    private static void OnOutputDataReceived(object? sender, DataReceivedEventArgs e)
    {
        var output = e.Data;

        _outputStream?.Write(Encoding.UTF8.GetBytes(output));

        //do something with your data
        Console.WriteLine("output:" + output);
    }

//Handle the error
    private static void OnErrorDataReceived(object sender, DataReceivedEventArgs e)
    {
        Trace.WriteLine(e.Data);
        //do something with your exception
        throw new Exception();
    }

// Handle Exited event and display process information.
    private static void OnExited(object? sender, EventArgs e)
    {
        Trace.WriteLine("Process exited");
    }

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

    /// <summary>
    /// Execute a shell command and return the output
    /// </summary>
    public static async Task CallCommand(string cmd, DataReceivedEventHandler callback = null!)
    {
        using var process = new Process();
        process.StartInfo.FileName = shell[0];
        process.StartInfo.Arguments = $"{shell[1]} \"{cmd}\"";

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

        // return await process.StandardOutput.ReadToEndAsync();
    }
}

public class CliException : Exception
{
    public CliException(string s) : base(s)
    {
    }
}
