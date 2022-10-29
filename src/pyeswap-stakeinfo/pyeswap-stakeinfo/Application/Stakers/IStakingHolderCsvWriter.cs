using System.Collections.Generic;
using System.Threading.Tasks;
using FluentResults;

namespace PYESwapStakeInfo.Application.Stakers;

internal interface IStakingHolderCsvWriter
{
    Task<Result> WriteAsync(string filename, IReadOnlyCollection<Staker> stakingHolders);
}