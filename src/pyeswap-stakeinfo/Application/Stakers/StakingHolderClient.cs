using System;
using System.Threading.Tasks;
using FluentResults;
using Nethereum.Contracts.ContractHandlers;
using Nethereum.Util;
using Nethereum.Web3;
using PYESwapStakeInfo.Application.Slices;

namespace PYESwapStakeInfo.Application.Stakers;

internal static class StakingHolderClient
{
    public static async Task<Result<Staker>> ReadHolderAsync(
        int chainId, string stakingContract, SliceHolder sliceHolder)
    {
        string rpcAddress = Chain.ToRpc(chainId).ToString();
        Web3 web3 = new(rpcAddress);
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