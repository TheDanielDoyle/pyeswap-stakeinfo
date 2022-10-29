using System;

namespace PYESwapStakeInfo.Application.Stakers;

internal sealed class CannotReadStakerException : Exception
{
    public CannotReadStakerException(string message) : base(message)
    {
    }
}