using System.Diagnostics;
using System.Text;
using OptDsl.Visitors;

namespace OptDsl.Drivers;

public class LlvmDriver
{
    public bool Execute(IReadOnlyList<TargetConfig> targetConfigs)
    {
        StringBuilder sb = new();
        string outputFile = "samples/a.out";

        foreach (var config in targetConfigs)
        {
            bool hasError = ExecuteFile(config);
            if (hasError)
            {
                return true;
            }
            string baseName = Path.GetFileNameWithoutExtension(config.TargetFile);
            sb.Append($"samples/{baseName}_opt.ll ");
        }
        Console.WriteLine("Running final clang to link all the optimized IR's to one optimized executable...");

        return Run("clang", $"{sb} -o \"{outputFile}\" -lpthread -ldl -lm");
    }

    private bool ExecuteFile(TargetConfig config)
    {
        string baseName = Path.GetFileNameWithoutExtension(config.TargetFile);
        string rawIr = $"samples/{baseName}.ll";
        string optIr = $"samples/{baseName}_opt.ll";

        Console.WriteLine("Running clang to emit raw llvm ir...");
        bool hasFailed =
            Run("clang", $"-S -emit-llvm -O0 -Xclang -disable-O0-optnone -DNDEBUG \"{config.TargetFile}\" -o \"{rawIr}\"");
        if (hasFailed)
        {
            return true;
        }

        Console.WriteLine("Running opt to optimize the raw llvm ir bases on dsl's configurations...");
        hasFailed =
            Run("opt", $"-passes=\"{config.PassPipeline}\" \"{rawIr}\" -S -o \"{optIr}\"");
        if (hasFailed)
        {
            return true;
        }

        Console.WriteLine($"Finished optimizing file \"{config.TargetFile}\"");

        return false;
    }

    private bool Run(string cmd, string args)
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
