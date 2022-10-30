using System.Collections.Generic;
using System.Threading.Tasks;
using FluentResults;
using Microsoft.Extensions.Logging;

namespace PYESwapStakeInfo.Application.Slices;

internal sealed class SliceHolderReader : ISliceHolderReader
{
    private readonly ISliceHolderClient _client;
    private readonly ILogger _logger;

    public SliceHolderReader(ISliceHolderClient client, ILogger<SliceHolderReader> logger)
    {
        _client = client;
        _logger = logger;
    }

    public Task<Result<IReadOnlyCollection<SliceHolder>>> ReadAsync(
        int chainId, string sliceContract, string covalentApiKey)
    {
         _logger.LogInformation("Reading holders from slice token {SliceContract} on chain {ChainId}", chainId, sliceContract);
        return _client.ReadAsync(chainId, sliceContract, covalentApiKey);
    }
}