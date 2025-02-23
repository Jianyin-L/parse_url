﻿using Parse_URL.Services;
using Parse_URL.Configs;
using Microsoft.Extensions.Configuration;

// Retrieve config and user inputs
var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true) // TODO: maybe optional is true here
    .Build();
var settings = SettingsRetriever.RetrieveConfigs(configuration);
var (filePath, topUrls, topIPs, filterMissing, includeTies) = SettingsRetriever.RetrieveInputs(args, settings);

// Parse log file
var logEntries = LogParser.ParseLogFile(filePath);
var topUrlsExcludeTies = LogStatistics.GetTopItems(logEntries, log => log.Url, topUrls, filterMissing, includeTies);
var topIPsIncludeTies = LogStatistics.GetTopItems(logEntries, log => log.IPAddress, topIPs, filterMissing, includeTies);

// Output results
Console.WriteLine(
    "=========================================\n" +
    "Settings:\n" +
    $"  File Path: {filePath}\n" +
    $"  Top N Most Visited URLs: {topUrls}\n" +
    $"  Top N Active IPs: {topIPs}\n" +
    $"  Filter out incomplete entries in responses: {filterMissing}\n" +
    $"  Include ties in responses: {includeTies}\n"
    );

Console.WriteLine("=========================================");
Console.WriteLine($"Total Number of Entries:{logEntries.Count}");

Console.WriteLine("\n=========================================");
Console.WriteLine($"Number of Unique IP Addresses:{LogStatistics.CountUniqueItems(logEntries, log => log.IPAddress)}");

Console.WriteLine("\n=========================================");
Console.WriteLine($"Top {topUrls} Most Visited URLs:");
foreach (var url in topUrlsExcludeTies)
{
    Console.WriteLine($"{url.Key}: {url.Value} times");
}

Console.WriteLine("\n=========================================");
Console.WriteLine($"Top {topIPs} Most Active IPs:");
foreach (var ip in topIPsIncludeTies)
{
    Console.WriteLine($"{ip.Key}: {ip.Value} times");
}