using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;

namespace PYESwapStakeInfo.Application.Stakers;

[Function("pendingReward", typeof(PendingRewardFunctionOutput))]
internal sealed class PendingRewardFunction : FunctionMessage
{
    [Parameter("address", "_user")]
    public string Address { get; set; }
}