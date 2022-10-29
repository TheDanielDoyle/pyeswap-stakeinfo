using System.Collections.Generic;
using Newtonsoft.Json;

namespace PYESwapStakeInfo.Application.Slices;

internal sealed record SliceHoldersDataDto
{
    [JsonProperty("items")]
    public IList<SliceHolderDto> Holders { get; set; }

    [JsonProperty("pagination")]
    public SliceHoldersPaginationDto Pagination { get; set; }

    public bool HasMore => Pagination?.HasMore ?? false;
}