using Parse_URL.Model;
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

//var parser = new LogParser();
//var results = parser.ParseLogFile(filePath);

//Console.WriteLine($"Unique IP addresses: {results.UniqueIPs}");
//Console.WriteLine("Top 3 Most Visited URLs:");

//// TODO-3: Refactor the following code to use a method that takes a dictionary and a number 'n' and returns the top 'n' items.
//// Can move the following inside the LogParser class instead of clustering the main method.
//foreach (var url in results.TopUrls)
//{
//    Console.WriteLine($"{url.Key}: {url.Value} times");
//}

//Console.WriteLine("Top 3 Most Active IPs:");
//foreach (var ip in results.TopIPs)
//{
//    Console.WriteLine($"{ip.Key}: {ip.Value} times");
//}

var parser = new LogParser();
List<LogEntry> logEntries = parser.ParseLogFile(filePath);

Console.WriteLine($"Total Entries: {logEntries.Count}");
Console.WriteLine($"Unique IP Addresses: {logEntries.Select(e => e.IPAddress).Distinct().Count()}");

var topUrls = logEntries
    .GroupBy(e => e.Url)
    .OrderByDescending(g => g.Count())
    .Take(3)
    .ToDictionary(g => g.Key, g => g.Count());

Console.WriteLine("Top 3 Most Visited URLs:");
foreach (var url in topUrls)
{
    Console.WriteLine($"{url.Key}: {url.Value} times");
}

var topIPs = logEntries
    .GroupBy(e => e.IPAddress)
    .OrderByDescending(g => g.Count())
    .Take(3)
    .ToDictionary(g => g.Key, g => g.Count());

Console.WriteLine("Top 3 Most Active IPs:");
foreach (var ip in topIPs)
{
    Console.WriteLine($"{ip.Key}: {ip.Value} times");
}