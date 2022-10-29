namespace PYESwapStakeInfo;

internal static class AppResult
{
    public const int ArgsError = -1;
    public const int Ok = 0;
    public const int ReadSliceHoldersError = 1;
    public const int ReadStakingHoldersError = 2;
    public const int WriteToCsvError = 3;
    public const int UnknownError = 4;
}