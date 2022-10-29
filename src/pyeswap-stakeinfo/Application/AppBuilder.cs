using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PYESwapStakeInfo.Application.Slices;
using PYESwapStakeInfo.Application.Stakers;

namespace PYESwapStakeInfo.Application;

internal static class AppBuilder
{
    public static IApp Build(Options options)
    {
        return new ServiceCollection()
            .AddLogging(o => o.AddConsole())
            .AddSingleton<IApp, App>()
            .AddSingleton<ISliceHolderReader, SliceHolderReader>()
            .AddSingleton<IStakingContractReader, StakingContractReader>()
            .AddSingleton<IStakingHolderCsvWriter, StakingHolderCsvWriter>()
            .AddSingleton<Options>(options)
            .BuildServiceProvider()
            .GetRequiredService<IApp>();
    }
}