using Parse_URL.Utilities;

var filePath = "./Data/example.log";
var logEntries = LogParser.ParseLogFile(filePath);

Console.WriteLine($"Total Number of Entries:{logEntries.Count}");
Console.WriteLine("=========================================\n");

Console.WriteLine($"Number of Unique IP Addresses:{LogStatistics.CountUniqueItems(logEntries, log => log.IPAddress)}");
Console.WriteLine("=========================================\n");

var topUrlsExcludeTies = LogStatistics.GetTopItems(logEntries, log => log.Url, 3, includeTies: false);
var topIPsIncludeTies = LogStatistics.GetTopItems(logEntries, log => log.IPAddress, 3, includeTies: true);

Console.WriteLine("Top 3 Most Visited URLs (EXCLUDING TIES):");
Console.WriteLine("=========================================");
foreach (var url in topUrlsExcludeTies)
{
    Console.WriteLine($"{url.Key}: {url.Value} times");
}

Console.WriteLine("\nTop 3 Most Active IPs (INCLUDING TIES):");
Console.WriteLine("=========================================");
foreach (var ip in topIPsIncludeTies)
{
    Console.WriteLine($"{ip.Key}: {ip.Value} times");
}