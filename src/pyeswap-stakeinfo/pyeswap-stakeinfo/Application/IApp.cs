using System.Threading.Tasks;

namespace PYESwapStakeInfo.Application;

internal interface IApp
{
    Task<int> RunAsync();
}