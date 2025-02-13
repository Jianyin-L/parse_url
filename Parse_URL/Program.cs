using Parse_URL.Services;

var filePath = "./Data/example.log";
var topUrls = 3;
var topIPs = 3;
var filterMissing = false;
var includeTies = false;

var logEntries = LogParser.ParseLogFile(filePath);
var topUrlsExcludeTies = LogStatistics.GetTopItems(logEntries, log => log.Url, topUrls, filterMissing, includeTies);
var topIPsIncludeTies = LogStatistics.GetTopItems(logEntries, log => log.IPAddress, topIPs, filterMissing, includeTies);

Console.WriteLine(
    "Settings:\n" +
    $"  File Path: {filePath}\n" +
    $"  Top URLs: {topUrls}\n" +
    $"  Top IPs: {topIPs}\n" +
    $"  Filter out entries if lacking requested field: {filterMissing}\n" +
    $"  Include ties: {includeTies}\n"
    );

Console.WriteLine("=========================================");
Console.WriteLine($"Total Number of Entries:{logEntries.Count}");

Console.WriteLine("\n=========================================");
Console.WriteLine($"Number of Unique IP Addresses:{LogStatistics.CountUniqueItems(logEntries, log => log.IPAddress)}");

Console.WriteLine("\n=========================================");
Console.WriteLine($"Top 3 Most Visited URLs:");
foreach (var url in topUrlsExcludeTies)
{
    Console.WriteLine($"{url.Key}: {url.Value} times");
}

Console.WriteLine("\n=========================================");
Console.WriteLine($"Top 3 Most Active IPs:");
foreach (var ip in topIPsIncludeTies)
{
    Console.WriteLine($"{ip.Key}: {ip.Value} times");
}