using Newtonsoft.Json;

namespace PYESwapStakeInfo.Application.Slices;

internal sealed record SliceHolderDto
{
    [JsonProperty("address")]
    public string Address { get; set; }
}