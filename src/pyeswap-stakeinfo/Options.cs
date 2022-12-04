using CommandLine;

namespace PYESwapStakeInfo;

public class Options
{
    [Value(index: 0, Required = true, HelpText = "Chain Id.")]
    public int ChainId { get; set; }

    [Value(index: 3, Required = true, HelpText = "Covalent API key. See https://www.covalenthq.com/")]
    public string CovalentApiKey { get; set; }

    [Value(index: 4, Required = true, HelpText = "File name")]
    public string Filename { get; set; }

    [Value(index: 1, Required = true, HelpText = "Slice contract address")]
    public string SliceContract { get; set; }

    [Value(index: 2, Required = true, HelpText = "Staking contract address")]
    public string StakingContract { get; set; }
}