using System.Diagnostics;

namespace test;

public class Tests
{
    [TestCaseSource(nameof(EnumerateDays))]
    public async Task RunSolutions(string path)
    {
        var msg = $"Running solution in {path}";
        var banner = new System.String(Enumerable.Repeat('-', msg.Length).ToArray());
        Console.WriteLine();
        Console.WriteLine(banner);
        Console.WriteLine(msg);
        Console.WriteLine(banner);
        Console.WriteLine();
        
        var p = Process.Start(new ProcessStartInfo("dotnet", "run --no-build")
        {
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            WorkingDirectory = path
        });
        Debug.Assert(p != null);
        await p.WaitForExitAsync();
        Console.WriteLine(await p.StandardOutput.ReadToEndAsync());
        var errorsOut = await p.StandardError.ReadToEndAsync();
        if (!String.IsNullOrWhiteSpace(errorsOut))
        {
            Console.WriteLine("StdErr out: was:\n----------");
            Console.WriteLine(errorsOut);
        }
        Assert.That(p.ExitCode, Is.EqualTo(0), $"Process did not exit with code 0, but {p.ExitCode}");
    }

    public static IEnumerable<string> EnumerateDays()
    {
        int maxDepth = 10;
        string dir = Environment.CurrentDirectory;
        bool found = false;
        while (!found && maxDepth > 0)
        {
            dir = Directory.GetParent(dir)?.FullName ?? throw new ApplicationException($"No parent for {dir}");
            var slnFiles = Directory.EnumerateFiles(dir, "*.sln");
            found = slnFiles.Any();
            maxDepth--;
        }

        if (!found)
            throw new ApplicationException("Unable to find sln dir");

        var directories = Directory.EnumerateDirectories(dir, "day*");
        return directories;
    }
}