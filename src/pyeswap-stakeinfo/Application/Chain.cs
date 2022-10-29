using System;

namespace PYESwapStakeInfo.Application;

internal static class Chain
{
    public static Uri ToRpc(int chainId)
    {
        return new Uri(chainId switch
        {
            1 => "https://rpc.ankr.com/eth",
            56 => "https://bsc-dataseed.binance.org/",
            _ => throw new ArgumentOutOfRangeException(nameof(chainId))
        });
    }
}