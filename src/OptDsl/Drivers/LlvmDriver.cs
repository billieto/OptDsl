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

        Run("clang", $"-S -emit-llvm -O0 -Xclang -disable-O0-optnone -DNDEBUG \"{targetFile}\" -o \"{rawIr}\"");

        Run("opt", $"-passes=\"{passes}\" \"{rawIr}\" -S -o \"{optIr}\"");

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

        proc.WaitForExit();

        if (proc.ExitCode != 0)
        {
            string err = proc.StandardError.ReadToEnd();
            throw new Exception($"Command '{cmd} {args}' failed with error:\n{err}");
        }
    }
}
