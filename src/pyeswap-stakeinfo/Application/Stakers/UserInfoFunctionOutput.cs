using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace PYESwapStakeInfo.Application.Stakers;

[FunctionOutput]
internal sealed record UserInfoFunctionOutput : IFunctionOutputDTO 
{
    [Parameter("uint256", "amount", 1)]
    public BigInteger Amount { get; set; }

    [Parameter("uint256", "rewardDebt ", 2)]
    public BigInteger RewardDebt { get; set; }

    [Parameter("uint256", "depositTime ", 3)]
    public BigInteger DepositTime  { get; set; }
}