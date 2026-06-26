using BenchmarkDotNet.Running;

namespace SimplexLab.Lwfix.TBenchmark
{
    /// <summary>
    /// 基准测试入口
    /// <para>用法：dotnet run -c Release -- --filter '*'
    /// 或指定过滤器：dotnet run -c Release -- --filter '*Div*'</para>
    /// </summary>
    internal static class Program
    {
        private static void Main(string[] args)
        {
            BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
        }
    }
}
