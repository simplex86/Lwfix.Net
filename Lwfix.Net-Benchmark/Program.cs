using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Toolchains.InProcess.Emit;

namespace SimplexLab.Lwfix.TBenchmark
{
    /// <summary>
    /// 基准测试入口
    /// <para>用法：dotnet run -c Release -- --filter '*'
    /// 或指定过滤器：dotnet run -c Release -- --filter '*Div*'</para>
    /// <para>使用 InProcessEmitToolchain 规避 Windows Defender 对子进程的干扰</para>
    /// </summary>
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var config = ManualConfig.Create(DefaultConfig.Instance)
                .AddJob(Job.Default
                    .WithWarmupCount(3)
                    .WithIterationCount(5)
                    .WithToolchain(InProcessEmitToolchain.Instance));
            BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args, config);
        }
    }
}
