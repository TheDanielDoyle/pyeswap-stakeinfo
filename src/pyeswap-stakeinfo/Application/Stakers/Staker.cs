using System.Numerics;

namespace PYESwapStakeInfo.Application.Stakers;

internal sealed record Staker(string Address, string StakingContract, BigInteger AmountInWei, BigInteger PendingRewardsInWei);