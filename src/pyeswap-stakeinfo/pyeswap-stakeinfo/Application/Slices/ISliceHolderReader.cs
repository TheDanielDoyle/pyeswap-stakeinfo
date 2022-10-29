using System.Collections.Generic;
using System.Threading.Tasks;
using FluentResults;

namespace PYESwapStakeInfo.Application.Slices;

internal interface ISliceHolderReader
{
    Task<Result<IReadOnlyCollection<SliceHolder>>> ReadAsync(
        int chainId, string sliceContract, string covalentApiKey);
}