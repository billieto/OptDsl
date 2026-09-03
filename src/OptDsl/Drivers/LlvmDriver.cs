using System.Diagnostics;
using System.Runtime.Serialization;

namespace OptDsl.Drivers;

public static class LlvmDriver
{
    public static bool Execute(string targetFile, string passes)
    {
        string baseName = Path.GetFileNameWithoutExtension(targetFile);
        string rawIr = $"samples/{baseName}.ll";
        string optIr = $"samples/{baseName}_opt.ll";
        string outputExecutable = $"samples/{baseName}";

        bool hasFailed = false;

        Console.WriteLine("Running clang to emit raw llvm ir...");
        hasFailed =
            Run("clang", $"-S -emit-llvm -O0 -Xclang -disable-O0-optnone -DNDEBUG \"{targetFile}\" -o \"{rawIr}\"");
        if (hasFailed)
        {
            return true;
        }

        Console.WriteLine("Running opt to optimize the raw llvm ir bases on dsl's configurations...");
        hasFailed =
            Run("opt", $"-passes=\"{passes}\" \"{rawIr}\" -S -o \"{optIr}\"");
        if (hasFailed)
        {
            return true;
        }

        Console.WriteLine("Running final clang to emit optimized executable...");
        hasFailed =
            Run("clang", $"\"{optIr}\" -o \"{outputExecutable}\" -lpthread -ldl -lm");
        if (hasFailed)
        {
            return true;
        }

        Console.WriteLine("Finished!");

        return false;
    }

    private static bool Run(string cmd, string args)
    {
        ProcessStartInfo procInfo = new()
        {
            FileName = cmd,
            Arguments = args,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        Process proc = new()
        {
            StartInfo = procInfo,
        };

        proc.Start();
        proc.WaitForExit();

        if (proc.ExitCode != 0)
        {
            string err = proc.StandardError.ReadToEnd();
            Console.WriteLine($"Command '{cmd} {args}' failed with error:\n{err}");
            return true;
        }

        return false;
    }
}
