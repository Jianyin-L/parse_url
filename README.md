# parse_url

## Log file structure
**Common Log Format (CLF)** and **Combined Log Format**, which are standard formats used by web servers like Apache and Nginx.  

Example:  
```
177.71.128.21 - - [10/Jul/2018:22:21:28 +0200] "GET /intranet-analytics/ HTTP/1.1" 200 3574 "-" "Mozilla/5.0 ..."
```

Each section represents a different piece of information:

**Field** | **Description**
--- | ---
177.71.128.21 | IP address of the client making the request
`- -` | Placeholder for identity and user authentication (typically unused)
[10/Jul/2018:22:21:28 +0200] | Timestamp of the request
"GET /intranet-analytics/ HTTP/1.1"	| HTTP request method, URL, and protocol
200	| HTTP status code (200 = OK)
3574 | Response size in bytes (excluding headers)
"-"	| Referrer (if empty, "-")
"Mozilla/5.0 ..." | User-Agent (browser or tool making the request)