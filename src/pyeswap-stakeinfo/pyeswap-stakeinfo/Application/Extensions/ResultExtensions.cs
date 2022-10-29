using System.Linq;
using FluentResults;

namespace PYESwapStakeInfo.Application.Extensions;

internal static class ResultExtensions
{
    private const string _errorSeparatorDefault = " ";

    public static string ToErrorString(this Result result, string separator = _errorSeparatorDefault)
    {
        return string.Join(separator, result.Errors.Select(e => e.Message));
    }
}