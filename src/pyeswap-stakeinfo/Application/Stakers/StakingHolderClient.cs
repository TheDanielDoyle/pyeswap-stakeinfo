using System.Net.Http;
using System.Threading.Tasks;
using FluentResults;
using Microsoft.Extensions.Logging;
using Nethereum.Contracts.ContractHandlers;
using Nethereum.JsonRpc.Client;
using Nethereum.Util;
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

        UserInfoFunction function = new()
        {
            Address = sliceHolder.Address
        };

        UserInfoFunctionOutput result = await contract
            .QueryAsync<UserInfoFunction, UserInfoFunctionOutput>(function)
            .ConfigureAwait(false);

        decimal amount = Web3.Convert.FromWei(result.Amount, UnitConversion.EthUnit.Gwei);

        _logger.LogInformation("Holder {Holder} on chain {ChainId} in staking contract {StakingContract} holds {Amount} tokens", 
            sliceHolder.Address, chainId, stakingContract, amount);

        return Result.Ok(new Staker(sliceHolder.Address, stakingContract, amount));
    }
}