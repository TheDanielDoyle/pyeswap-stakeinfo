using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;

namespace PYESwapStakeInfo.Application.Stakers;

[Function("userInfo", typeof(UserInfoFunctionOutput))]
internal sealed class UserInfoFunction : FunctionMessage
{
    [Parameter("address", "address")]
    public string Address { get; set; }
}