using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using FluentResults;

namespace PYESwapStakeInfo.Application.Stakers;

internal sealed class StakingHolderCsvWriter : IStakingHolderCsvWriter
{
    public async Task<Result> WriteAsync(string filename, IReadOnlyCollection<Staker> stakingHolders)
    {
        CsvConfiguration configuration = new(CultureInfo.InvariantCulture)
        {
            NewLine = Environment.NewLine
        };

        try
        {
            await using FileStream fileStream = File.Open(filename, FileMode.Create);
            await using StreamWriter writer = new(fileStream);
            await using CsvWriter csv = new(writer, configuration);
            await csv
                .WriteRecordsAsync(stakingHolders.OrderByDescending(e => e.Amount))
                .ConfigureAwait(false);

            return Result.Ok();
        }
        catch (Exception exception)
        {
            return Result.Fail(exception.Message);
        }
    }
}