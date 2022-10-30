using System;
using System.Net.Http;
using System.Threading.Tasks;
using FluentResults;
using Nethereum.Contracts.ContractHandlers;
using Nethereum.JsonRpc.Client;
using Nethereum.Util;
using Nethereum.Web3;
using PYESwapStakeInfo.Application.Slices;

namespace PYESwapStakeInfo.Application.Stakers;

internal sealed class StakingHolderClient : IStakingHolderClient
{
    private readonly HttpClient _client;

    public StakingHolderClient(HttpClient client)
    {
        _client = client;
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

        UserInfoFunctionOutput result = await contract.QueryAsync<UserInfoFunction, UserInfoFunctionOutput>(function);
        decimal amount = Web3.Convert.FromWei(result.Amount, UnitConversion.EthUnit.Gwei);

        Console.WriteLine("Holder {0} on chain {1} in staking contract {2} holds {3} tokens", 
            sliceHolder.Address, chainId, stakingContract, amount);

        return Result.Ok(new Staker(sliceHolder.Address, stakingContract, amount));
    }
}