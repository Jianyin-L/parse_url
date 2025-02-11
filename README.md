# Log Analyser - C# Console App :pager:

## Overview :page_with_curl:
This is a simple console application written in C# that reads a log file and generates useful statistics based on the data. 

Sample log file:
```
177.71.128.21 - admin [10/Jul/2018:22:21:28 +0200] "GET /intranet-analytics/ HTTP/1.1" 200 3574 "-" "Mozilla/5.0 ..."
79.125.00.21 - - [10/Jul/2018:20:03:40 +0200] "GET /newsletter/ HTTP/1.1" 200 3574 "-" "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; Trident/5.0)"
```
> You can read more about this log file structure in the [Reference](#reference) section.

## Objectives :dart:
The goal of this application is to answer three main questions: 
- What are the number of unique IP addresses?
- Which are the top 3 most visited URLs?
- Which are the top 3 most active IP addresses?

## Techstack :computer:
- **Language**: C#
- **.Net Version**: .Net 8.0 (*It should work on older .NET versions as well.*)
- **Libraries**: 
    - `System.IO` (for file reading)
    - `Console` (for console output)
    - `LINQ` (for data manipulation)
    - `System.Globalization` (for DateTime parsing)
    - `System.Text.RegularExpressions` (for Regex)

## Project Structure :open_file_folder:
The project structure is as follows:
```bash
.
├── Data
│   └── example.log
├── LogAnalyser.csproj
├── Program.cs
└── README.md
```
- `Data`: Contains the sample log file.

TODO: Update this after the project structure is finalised.

## Features :sparkles:
The `Program.cs` file is the entry point of the application and you can customise the application's behaviour by modifying this file in the following ways: 

### 1. Read a Different Log File
The application reads the log file `./Data/example.log`. This can be updated to any other log file as long as it follows the log structure mentioned in the [Reference](#reference) section.
```csharp
var filePath = "path/to/your/logfile.log";  // Update this path to your log file
```
### 2. Customise the Number of Items to Return
The application is set to return the top 3 most visited URLs and the top 3 most active IP addresses, which the number can be adjusted in the `Program.cs` as follows:
```csharp
var topUrls = 2;
var topIPs = 2;
```

### 3. How to Handle Ties
The application does not include ties when displaying the top URLs and IPs by default.  
To include ties, set the following variable to `true` in the `Program.cs`:
```csharp
var includeTies = true;
```

### 4. How to Handle Missing Required Fields
The application does not filter out entries if the entry is missing a required field. For example, if the log entry does not contain a URL, it will still be considered as a valid entry when calculating the top URLs.  
To filter out such entries, set the following variable to `true`:
```csharp
var filterMissing = true;
```

*Excited to try it out? Follow the following [instructions](#setup).*

## Setup :wrench:
### Prerequisites
- Install [.NET Core SDK](https://dotnet.microsoft.com/download) on your machine.

### Steps
1. Clone this repository. 
    ```bash
    git clone https://github.com/Jianyin-L/parse_url.git
    ```
2. Navigate to the project directory.  
    A sample data file is located in the `./Data` folder. 
    
    TODO: move the file around so it is not nested Parse_URL

3. Build and run the project in Command Line. 
    ```bash
    dotnet build
    dotnet run
    ```
    The output will be displayed in the console.  

## Expected Output :chart_with_upwards_trend:
TODO: Update the output once the project is finalised   
The output without any customisation will look like this:
```
Settings:
  File Path: ./Data/example.log
  Top URLs: 3
  Top IPs: 3
  Filter out entries if lacking requested field: False
  Include ties: False

=========================================
Total Number of Entries:23

=========================================
Number of Unique IP Addresses:11

=========================================
Top 3 Most Visited URLs:
/docs/manage-websites/: 2 times
/intranet-analytics/: 1 times
http://example.net/faq/: 1 times

=========================================
Top 3 Most Active IPs:
168.41.191.40: 4 times
177.71.128.21: 3 times
50.112.00.11: 3 times
```
which means: 
- The log file contains 23 entries.
- There are 11 unique IP addresses. 
- The top 3 most visited URLs, excluding ties, are 
	- `/docs/manage-websites/`: 2 times
	- `/intranet-analytics/`: 1 time
	- `http://example.net/faq/`: 1 time
- The top 3 most visited URLs, including  ties, are
    - `168.41.191.40`: 4 times
    - `177.71.128.21`: 3 times
    - *and more*...

## Future Improvements :rocket:
### Command Line Arguments
- Allow the user to specify the file path as a command-line argument.
- Support other arguments, such as specifying the number of top URLs and IPs to display, whether to include ties, etc. 

### Data Handling
- Support parsing error logs, not just 2xx success logs.
    ```bash
    // Example error log
    [Fri Jul 14 14:32:14.873076 2024] [core:notice] [pid 1234] AH00094: Command line: '/usr/sbin/httpd -D FOREGROUND'
    ```
- Better handle various data structures, such as handling IP addresses in IPv6 format, handling different date formats, etc.

## Reference :books:
### Log structure
**Common Log Format (CLF)** and **Combined Log Format** are standard formats used by web servers like Apache and Nginx.  

Example log:  
```
177.71.128.21 - admin [10/Jul/2018:22:21:28 +0200] "GET /intranet-analytics/ HTTP/1.1" 200 3574 "-" "Mozilla/5.0 ..."
```

Each section represents a different piece of information:

**Field** | **Description**
--- | ---
177.71.128.21 | IP address of the client making the request
`-` | Remote log name (typically unused)
admin | User Name
[10/Jul/2018:22:21:28 +0200] | Timestamp of the request
"GET /intranet-analytics/ HTTP/1.1"	| HTTP request method, URL, and protocol
200	| HTTP status code
3574 | Response size in bytes (excluding headers)
`-`	| Referrer, which page a visitor was on before arrived at the site (if empty, "-")
"Mozilla/5.0 ..." | User-Agent (browser or tool making the request)

Reading more here: https://signoz.io/guides/apache-log/
