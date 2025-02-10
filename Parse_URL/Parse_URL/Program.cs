using Parse_URL.Utilities;

//// TODO-1: Allow the user to specify the log file path as a command line argument. If not provided, use the default file path.
//// TODO-2: Move this as a method in the LogParser class and call it from the Main method.
//if (args.Length == 0)
//{
//    Console.WriteLine("Usage: parse_URL <logfile>");
//    return;
//}

//string filePath = args[0];

//if (!File.Exists(filePath))
//{
//    Console.WriteLine("Error: File not found.");
//    return;
//}

var filePath = "./Data/example.log";
var logEntries = LogParser.ParseLogFile(filePath);

//foreach (var log in logEntries)
//{
//    Console.WriteLine($"{log.IPAddress} {log.User} {log.Timestamp} {log.HttpMethod} {log.Url} {log.StatusCode} {log.ResponseSize} {log.UserAgent}");
//}

Console.WriteLine($"Total Entries: {logEntries.Count}");
Console.WriteLine($"Unique IP Addresses: {LogStatistics.CountUniqueItems(logEntries, log => log.IPAddress)}");

var topUrls = LogStatistics.GetTopItems(logEntries, log => log.Url, 3);
Console.WriteLine("Top 3 Most Visited URLs:");
foreach (var url in topUrls)
{
    Console.WriteLine($"{url.Key}: {url.Value} times");
}

var topIPs = LogStatistics.GetTopItems(logEntries, log => log.IPAddress, 3);
Console.WriteLine("Top 3 Most Active IPs:");
foreach (var ip in topIPs)
{
    Console.WriteLine($"{ip.Key}: {ip.Value} times");
}