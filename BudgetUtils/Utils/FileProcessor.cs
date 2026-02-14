// Thanks MS: https://learn.microsoft.com/en-us/dotnet/api/system.io.directory.getfiles?view=net-10.0
// For Directory.GetFiles and Directory.GetDirectories
// For File.Exists, Directory.Exists
namespace BudgetUtils.Utils
{
    using System;
    using System.IO;
    using System.Collections.Generic;
    using BudgetUtils.Config;
    using Microsoft.Extensions.Configuration;

    public class RecursiveFileProcessor(AppSettings appSettings)
    {
        private readonly AppSettings _appSettings = appSettings;

        public List<string> ProcessDirectory(string targetDirectory, List<string>? recursedFiles = null)
        {
            // Initialize the list if it's null
            if (recursedFiles == null)
            {
                recursedFiles = new List<string>();
            }

            // Process the list of files found in the directory.
            string[] fileEntries = Directory.GetFiles(targetDirectory);
            foreach (string fileName in fileEntries)
            {
                string processedFile = ProcessFile(fileName);
                if (!String.IsNullOrEmpty(processedFile))
                {
                    recursedFiles.Add(processedFile);
                }
            }

            // Recurse into subdirectories of this directory.
            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            foreach (string subdirectory in subdirectoryEntries)
            {
                ProcessDirectory(subdirectory, recursedFiles);
            }

            return recursedFiles;
        }

        public string ProcessFile(string path)
        {
            DateTime dateTime = DateTime.MinValue;
            if (_appSettings != null)
            {
                if (_appSettings.DateRange != null)
                {
                    dateTime = _appSettings.DateRange.GetDate();
                }
            }

            if (Path.GetExtension(path).ToLower() != ".csv")
            {
                Console.WriteLine("Skipping non-.csv file at '{0}'.", path);
                return String.Empty;
            }
            if (File.GetLastAccessTime(path) < dateTime)
            {
                Console.WriteLine("Skipping .csv file at '{0}' because it was last accessed on {1}, which is before the cutoff date of {2}.", path, File.GetLastAccessTime(path), dateTime);
                return String.Empty;
            }
            Console.WriteLine("Found .csv file at '{0}'.", path);
            return path;
        }
    }
}