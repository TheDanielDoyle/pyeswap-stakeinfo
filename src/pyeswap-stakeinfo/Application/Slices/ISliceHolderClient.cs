using System.Collections.Generic;
using System.Threading.Tasks;
using FluentResults;

namespace PYESwapStakeInfo.Application.Slices;

internal interface ISliceHolderClient
{
    Task<Result<IReadOnlyCollection<SliceHolder>>> ReadAsync(
        int chainId, string sliceContract, string covalentApiKey);
}