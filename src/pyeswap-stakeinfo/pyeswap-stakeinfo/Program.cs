using System.Threading.Tasks;
using CommandLine;
using PYESwapStakeInfo.Application;

namespace PYESwapStakeInfo;

internal sealed class Program
{
    private static async Task<int> Main(string[] args)
    {
        return await Parser.Default.ParseArguments<Options>(args)
            .MapResult(async options =>
                {
                    IApp app = AppBuilder.Build(options);

                    return await app.RunAsync();
                },
                errors => Task.FromResult(AppResult.ArgsError));
    }
}