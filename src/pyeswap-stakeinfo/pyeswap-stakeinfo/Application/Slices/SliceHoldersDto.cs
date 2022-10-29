using Newtonsoft.Json;

namespace PYESwapStakeInfo.Application.Slices;

internal sealed record SliceHoldersDto
{
    [JsonProperty("data")]
    public SliceHoldersDataDto Data { get; set; }

    public bool HasMore => Data?.HasMore ?? false;
}
