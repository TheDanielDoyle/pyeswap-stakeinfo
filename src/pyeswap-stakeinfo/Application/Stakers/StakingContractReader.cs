using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentResults;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PYESwapStakeInfo.Application.Slices;

namespace PYESwapStakeInfo.Application.Stakers;

internal sealed class StakingContractReader : IStakingContractReader
{
    private readonly ILogger _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public StakingContractReader(ILogger<StakingContractReader> logger, IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task<Result<IReadOnlyCollection<Staker>>> ReadHoldersAsync(
        int chainId, string stakingContract, IReadOnlyCollection<SliceHolder> sliceHolders)
    {
        ConcurrentBag<Staker> stakingHolders = new();

        try
        {
            await Parallel.ForEachAsync(sliceHolders, async (sliceHolder, _) =>
            {
                using IServiceScope scope = _serviceScopeFactory.CreateScope();
                IStakingHolderClient client = scope.ServiceProvider.GetRequiredService<IStakingHolderClient>();
                Result<Staker> readStakingHolder =
                    await client
                        .ReadHolderAsync(chainId, stakingContract, sliceHolder)
                        .ConfigureAwait(false);

                if (readStakingHolder.IsFailed)
                {
                    throw CreateCannotReadStakerException(chainId, stakingContract, sliceHolder.Address);
                }

                stakingHolders.Add(readStakingHolder.Value);
            }).ConfigureAwait(false);
        }
        catch (CannotReadStakerException exception)
        {
            return Result.Fail(exception.Message);
        }
        return stakingHolders.ToList();
    }

    private CannotReadStakerException CreateCannotReadStakerException(
        int chainId, string stakingContract, string stakerAddress)
    {
        return new CannotReadStakerException(
            $"Cannot read staker {stakerAddress} on chain {chainId} for staking contract {stakingContract}");
    }
}