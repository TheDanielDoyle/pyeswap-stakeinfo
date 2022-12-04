using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace PYESwapStakeInfo.Application.Stakers;

[FunctionOutput]
internal sealed class PendingRewardFunctionOutput : IFunctionOutputDTO
{
    [Parameter("uint256", "", 1)]
    public BigInteger Amount { get; set; }
}