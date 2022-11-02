using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentResults;
using Microsoft.Extensions.Logging;
using PYESwapStakeInfo.Application.Slices;

namespace PYESwapStakeInfo.Application.Stakers;

internal sealed class StakingContractReader : IStakingContractReader
{
    private readonly IStakingHolderClient _client;
    private readonly ILogger _logger;

    public StakingContractReader(IStakingHolderClient client, ILogger<StakingContractReader> logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task<Result<IReadOnlyCollection<Staker>>> ReadHoldersAsync(
        int chainId, string stakingContract, IReadOnlyCollection<SliceHolder> sliceHolders)
    {
        ConcurrentBag<Staker> stakingHolders = new();

        try
        {
            await Parallel.ForEachAsync(sliceHolders, async (sliceHolder, _) =>
            {
                Result<Staker> readStakingHolder =
                    await _client
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