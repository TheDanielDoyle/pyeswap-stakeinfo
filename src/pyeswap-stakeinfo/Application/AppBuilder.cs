using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PYESwapStakeInfo.Application.Slices;
using PYESwapStakeInfo.Application.Stakers;

namespace PYESwapStakeInfo.Application;

internal static class AppBuilder
{
    public static IApp Build(Options options)
    {
        ServiceCollection services = new();

        services
            .AddHttpClient<ISliceHolderClient, SliceHolderClient>();

        services
            .AddHttpClient<IStakingHolderClient, StakingHolderClient>();

        services
            .AddLogging(logging => logging.AddConsole())
            .AddSingleton<IApp, App>()
            .AddSingleton<ISliceHolderReader, SliceHolderReader>()
            .AddSingleton<IStakingContractReader, StakingContractReader>()
            .AddSingleton<IStakingHolderCsvWriter, StakingHolderCsvWriter>()
            .AddSingleton<Options>(options);

        return services
            .BuildServiceProvider()
            .GetRequiredService<IApp>();
    }
}