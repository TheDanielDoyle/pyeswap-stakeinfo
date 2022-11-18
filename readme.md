# PYE Swap Staking Info

Output staker information from PYE Swap staking pools into a CSV file.

---

## Requirements

1. Microsoft .NET 7.0 or above
   * Download for your operating system from: https://dotnet.microsoft.com/en-us/download

2. CovalentHQ API key to read the holder list for the slice tokens.
   * Get a free key at: [https://www.covalenthq.com/](https://www.covalenthq.com/)

3. Build from source.
   * See below.

4. Staking and Slice contracts for the staking pool you wish to read information from.

---

## Running the app

After building from the source you may run the app from a terminal as follows

pyeswap-stakeinfo.exe [**ChainID**] [**Slice-Contract**] [**Staking-Contract**] [**CovalentHQ-API-Key**] [**Output-Filename**]

### Example:

```
pyeswap-stakeinfo.exe 56 0xd1510af5614ac683906e0dfe63fa192d18c0ae3f 0x76cb562a83c71b8457c9705072201740a4463100 ckey_d0vhuy9d54plvt4eqxuwj390tds pyeswap-bsc-pye-to-apple-pool.csv
```

They API Key above is a dummy. You need your own!

---

## Reading the numbers

The numbers come in BitIntger, therefore you are required to convert them to the decimal value you wish.

For example, Apple, which has 18 decimal places, you can run the following formula in Excel/Sheets to get the decimal value.

* =**CELL**/1E+18

Replace **CELL** with the cell reference. The example is essentially dividing the **CELL** by 18 zeros, or 000,000,000,000,000,000 to get the true value.

## Build from source

Run the following in the command line.

```
dotnet publish --runtime win-x64 --configuration Release --framework net6.0 --self-contained true -p:"PublishReadyToRun=true;PublishSingleFile=true;PublishTrimmed=true"
```

Replace the **--runtime** value to match your operating system.

### List of RIDs

* **win-x64**
* **win-arm64**
* **osx-x64**
* **linux-x64**
* **linux-arm64**

View the full list here:
[https://learn.microsoft.com/en-us/dotnet/core/rid-catalog](https://learn.microsoft.com/en-us/dotnet/core/rid-catalog)

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE.md) file for details.
