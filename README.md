
<a id="readme-top"></a>
<h1 align="center">FolderSyncApp</h1>

<!-- ABOUT THE PROJECT -->  
## About The Project
Simple Console App that is mirroring one directory into other, running in set intervals and ensuring an exact replica of source folder is maintained build with:<br>

[![Dotnet][.NET]][.NET-url]
<!-- GETTING STARTED -->  
## Getting Started
Here is instructions on how to build and run this application, and what prerequisites it needs in order to function correctly.

### Prerequisites
- [.NET 10 SDK](https://dotnet.microsoft.com/download)
### Build and run
```
dotnet build
dotnet run --project FolderSyncApp -- <sourceFolderPath> <replicaFolderPath> <logFilePath> <interval>
```
Example – synchronize `C:\Data` into `D:\Backup\Data` every 30 seconds:

```
dotnet run --project FolderSyncApp -- "C:\Data" "D:\Backup\Data" "D:\Logs\sync.log" 30
```
Show help:
```
dotnet run --project FolderSyncApp -- --help
```

<!-- USAGE EXAMPLES -->  
## Usage
While running this app pass 4 arguments (or 1 if you want to display help message). Those arguments are **sourceFolderPath** **replicaFolderPath** **logFilePath** **interval**
* **sourceFolderPath** - Path to the folder you wish to replicate
* **replicaFolderPath** - Path to the destination at which you wish to create the replica - if non existing folder is passed, it will be created at specified path (if possible)
* **logFilePath** - Path to the log file. File will be created if does not exist when running this application.
* **interval** - Interval - accept only full natural numbers and represents interval between synchronizations (in seconds).

You can also use arguments **-h** or **--help** to print out help information.
### Error handling
Application will handle simple errors. If passed arguments are not valid, it will communicate where the issue is and then stop, giving you the opportunity to fix arguments and start application again.
#### Argument parsing include:
* Checking whether any provided path point to the same location or file
* Checking whether any provided path is nested inside other (including log file)
* Checking whether interval provided is full natural number that parse to integer
#### Error handling while synchronizing:
While running, any **IOException** and **UnauthorizedAccessException** will be logged to file and console, file will be skipped, and application will continue.

<!-- CONTACT -->  
## Contact

Bartosz Bednarczyk - [@LinkedIn](https://www.linkedin.com/in/bartosz-bednarczyk-1833b216b) - bartosz.bednarczyk@outlook.com

Project Link: [https://github.com/Berni017/FolderSyncApp](https://github.com/Berni017/FolderSyncApp)

<p align="right">(<a href="#readme-top">back to top</a>)</p>  


<!-- MARKDOWN LINKS & IMAGES -->  
[.NET]: https://img.shields.io/badge/.NET-512BD4?logo=dotnet&logoColor=fff
[.NET-url]: https://learn.microsoft.com/en-us/dotnet/