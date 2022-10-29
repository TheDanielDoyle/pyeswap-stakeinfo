using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentResults;
using Microsoft.Extensions.Logging;
using PYESwapStakeInfo.Application.Extensions;
using PYESwapStakeInfo.Application.Slices;
using PYESwapStakeInfo.Application.Stakers;

namespace PYESwapStakeInfo.Application;

internal sealed class App : IApp
{
    private readonly ILogger _logger;
    private readonly IStakingHolderCsvWriter _csvWriter;
    private readonly Options _options;
    private readonly ISliceHolderReader _sliceReader;
    private readonly IStakingContractReader _stakingReader;

    public App(
        IStakingHolderCsvWriter csvWriter, 
        ILogger<App> logger,
        Options options, 
        ISliceHolderReader sliceReader, IStakingContractReader stakingReader)
    {
        _csvWriter = csvWriter;
        _logger = logger;
        _options = options;
        _sliceReader = sliceReader;
        _stakingReader = stakingReader;
    }

    public async Task<int> RunAsync()
    {
        try
        {
            Result<IReadOnlyCollection<SliceHolder>> readSliceHolders
                = await _sliceReader.ReadAsync(_options.ChainId, _options.SliceContract, _options.CovalentApiKey);
                        
            if (readSliceHolders.IsFailed)
            {
                _logger.LogError("Unable to read slice holders.");
                return AppResult.ReadSliceHoldersError;
            }
                        
            Result<IReadOnlyCollection<Staker>> readStakingHolders 
                = await _stakingReader.ReadHoldersAsync(_options.ChainId, _options.StakingContract, readSliceHolders.Value);
                        
            if (readStakingHolders.IsFailed)
            {
                _logger.LogError("Unable to read staking contract holders.");
                return AppResult.ReadStakingHoldersError;
            }

            Result writeToCsv = await _csvWriter.WriteAsync(_options.Filename, readStakingHolders.Value);
                        
            if (writeToCsv.IsFailed)
            {
                _logger.LogError("Unable to write to CSV. {Error}", writeToCsv.ToErrorString());
                return AppResult.WriteToCsvError;
            }

            return AppResult.Ok;
        }
        catch (Exception exception)
        {
            _logger.LogError("Unknown error! {0}", exception.Message);
            return AppResult.UnknownError;
        }
    }
}