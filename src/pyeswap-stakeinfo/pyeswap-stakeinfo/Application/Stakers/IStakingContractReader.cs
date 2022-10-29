using System.Collections.Generic;
using System.Threading.Tasks;
using FluentResults;
using PYESwapStakeInfo.Application.Slices;

namespace PYESwapStakeInfo.Application.Stakers;

internal interface IStakingContractReader
{
    Task<Result<IReadOnlyCollection<Staker>>> ReadHoldersAsync(
        int chainId, string stakingContract, IReadOnlyCollection<SliceHolder> sliceHolders);
}