using System.Net.Http;
using System.Numerics;
using System.Threading.Tasks;
using FluentResults;
using Microsoft.Extensions.Logging;
using Nethereum.Contracts.ContractHandlers;
using Nethereum.JsonRpc.Client;
using Nethereum.Web3;
using PYESwapStakeInfo.Application.Slices;

namespace PYESwapStakeInfo.Application.Stakers;

internal sealed class StakingHolderClient : IStakingHolderClient
{
    private readonly HttpClient _client;
    private readonly ILogger _logger;

    public StakingHolderClient(HttpClient client, ILogger<StakingHolderClient> logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task<Result<Staker>> ReadHolderAsync(
        int chainId, string stakingContract, SliceHolder sliceHolder)
    {
        RpcClient rpcClient = new(Chain.ToRpc(chainId), _client);
        Web3 web3 = new(rpcClient);
        ContractHandler contract = web3.Eth.GetContractHandler(stakingContract);

        UserInfoFunction userinfoFunction = new()
        {
            Address = sliceHolder.Address
        };

        UserInfoFunctionOutput userInfo = await contract
            .QueryAsync<UserInfoFunction, UserInfoFunctionOutput>(userinfoFunction)
            .ConfigureAwait(false);

        _logger.LogInformation("Holder {Holder} on chain {ChainId} in staking contract {StakingContract} holds {Amount} tokens", 
            sliceHolder.Address, chainId, stakingContract, userInfo.Amount);

        PendingRewardFunction pendingRewardFunction = new()
        {
            Address = sliceHolder.Address
        };
        
        PendingRewardFunctionOutput pendingReward = await contract
            .QueryAsync<PendingRewardFunction, PendingRewardFunctionOutput>(pendingRewardFunction)
            .ConfigureAwait(false);

        return Result.Ok(new Staker(sliceHolder.Address, stakingContract, userInfo.Amount, pendingReward.Amount));
    }
}