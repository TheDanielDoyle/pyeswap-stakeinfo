using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentResults;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
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
                AsyncRetryPolicy policy = BuildRetryPolicy(chainId, sliceHolder.Address, stakingContract);
                PolicyResult<Staker> readHolder = await ReadHolderAsync(policy, chainId, sliceHolder, stakingContract);

                if (readHolder.Outcome == OutcomeType.Failure)
                {
                    throw CreateCannotReadStakerException(chainId, stakingContract, sliceHolder.Address);
                }

                stakingHolders.Add(readHolder.Result);
            });
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

    private Task<PolicyResult<Staker>> ReadHolderAsync(
        IAsyncPolicy policy, int chainId, SliceHolder sliceHolder, string stakingContract)
    {
        return policy.ExecuteAndCaptureAsync<Staker>(async () =>
        {
            Result<Staker> readStakingHolder =
                await _client.ReadHolderAsync(chainId, stakingContract, sliceHolder);
            return readStakingHolder.Value;
        });
    }

    private AsyncRetryPolicy BuildRetryPolicy(int chainId, string stakerAddress, string stakingContract)
    {
        return Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), onRetry:
                (exception, timeSpan) =>
                {
                    _logger.LogInformation(
                        "Retrying reading staker information for staker {StakerAddress} " +
                        "on chain {ChainId} on staking contract {StakingContract}", 
                        stakerAddress, chainId, stakingContract);
                });
    }
}