using System.Threading.Tasks;
using FluentResults;
using PYESwapStakeInfo.Application.Slices;

namespace PYESwapStakeInfo.Application.Stakers;

internal interface IStakingHolderClient
{
    Task<Result<Staker>> ReadHolderAsync(
        int chainId, string stakingContract, SliceHolder sliceHolder);
}