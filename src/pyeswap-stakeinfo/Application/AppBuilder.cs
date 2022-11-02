using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PYESwapStakeInfo.Application.Slices;
using PYESwapStakeInfo.Application.Stakers;
using Polly;
using Polly.Extensions.Http;
using Polly.Retry;

namespace PYESwapStakeInfo.Application;

internal static class AppBuilder
{
    public static IApp Build(Options options)
    {
        ServiceCollection services = new();

        AsyncRetryPolicy<HttpResponseMessage> retryPolicy = HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

        services
            .AddHttpClient<ISliceHolderClient, SliceHolderClient>()
            .AddPolicyHandler(retryPolicy);

        services
            .AddHttpClient<IStakingHolderClient, StakingHolderClient>()
            .AddPolicyHandler(retryPolicy);

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