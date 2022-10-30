using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FluentResults;
using Flurl;
using Flurl.Http;
using Newtonsoft.Json;

namespace PYESwapStakeInfo.Application.Slices;

internal class SliceHolderClient : ISliceHolderClient
{
    private readonly IFlurlClient _client;

    public SliceHolderClient(HttpClient client)
    {
        _client = new FlurlClient(client);
    }

    public async Task<Result<IReadOnlyCollection<SliceHolder>>> ReadAsync(
        int chainId, string sliceContract, string covalentApiKey)
    {
        Url url = new Url("https://api.covalenthq.com/v1/")
            .AppendPathSegment(chainId)
            .AppendPathSegment("tokens")
            .AppendPathSegment(sliceContract)
            .AppendPathSegment("token_holders/")
            .SetQueryParam("key", covalentApiKey);

        bool hasMore;
        int pageNumber = 0;
        List<SliceHolder> sliceHolders = new();

        do
        {
            IFlurlResponse response = await _client.Request(url)
                .AllowAnyHttpStatus()
                .SetQueryParam("page-size", 1000)
                .SetQueryParam("page-number", pageNumber)
                .GetAsync();

            HttpResponseMessage message = response.ResponseMessage;

            if (!message.IsSuccessStatusCode)
            {
                return Result.Fail("Unable to read staking holders");
            }
            
            string content = await message.Content.ReadAsStringAsync();

            SliceHoldersDto dto = JsonConvert.DeserializeObject<SliceHoldersDto>(content);
            sliceHolders.AddRange(dto.Data.Holders.Select(h => new SliceHolder(h.Address)));

            hasMore = dto.HasMore;
            pageNumber += 1;

        } while (hasMore);


        return sliceHolders;
    }
}