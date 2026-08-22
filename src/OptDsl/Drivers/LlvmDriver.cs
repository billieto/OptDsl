using System.Diagnostics;

namespace OptDsl.Drivers;

public static class LlvmDriver
{
    public static void Execute(string targetFile, string passes)
    {
        string baseName = Path.GetFileNameWithoutExtension(targetFile);
        string rawIr = $"{baseName}.ll";
        string optIr = $"{baseName}_opt.ll";
        string outputExecutable = $"{baseName}";

        Console.WriteLine("Running clang to emit raw llvm ir...");
        Run("clang", $"-S -emit-llvm -O0 -Xclang -disable-O0-optnone -DNDEBUG \"{targetFile}\" -o \"{rawIr}\"");

        Console.WriteLine("Running opt to optimize the raw llvm ir bases on dsl's configurations...");
        Run("opt", $"-passes=\"{passes}\" \"{rawIr}\" -S -o \"{optIr}\"");
        
        Console.WriteLine("Running final clang to emit optimized executable...");
        Run("clang", $"\"{optIr}\" -o \"{outputExecutable}\" -lpthread -ldl -lm");
    }

    private static void Run(string cmd, string args)
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
            throw new Exception($"Command '{cmd} {args}' failed with error:\n{err}");
        }
    }
}
