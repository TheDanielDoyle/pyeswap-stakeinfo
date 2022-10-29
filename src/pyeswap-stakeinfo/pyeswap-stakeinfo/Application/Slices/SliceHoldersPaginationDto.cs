using Newtonsoft.Json;

namespace PYESwapStakeInfo.Application.Slices;

internal sealed record SliceHoldersPaginationDto
{
    [JsonProperty("has_more")]
    public bool HasMore { get; set; }

    [JsonProperty("page_number")]
    public int PageNumber { get; set; }

    [JsonProperty("page_size")]
    public int PageSize { get; set; }
}