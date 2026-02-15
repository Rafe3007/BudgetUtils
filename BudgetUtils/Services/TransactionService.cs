using BudgetUtils.Parsers;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetUtils.Services
{
    internal class TransactionService(string inputDirectory)
    {
        readonly string _inputDirectory = inputDirectory;

        // Fetches transactions from the data source and processes them as needed.
        public void RunService()
        {
            // Get Transaction Sources
            List<string> files = RecursiveFileProcessor.ProcessDirectory(_inputDirectory);

            // For each source, Identify which transaction parser to use and process them
            //   - If source does not match any known parser, log a warning and attempt to run generic parser on it,
            //     - if that fails, log an error and skip the file.
            // Create a single output file with all transactions in the standardized format.
            foreach (string file in files)
            {
                var parser = TransactionParserFactory.GetParser(file);
                try
                {
                    var transactions = parser.ParseTransactions(file);
                    if (transactions != null && transactions.Any())
                    {
                        //foreach (var transaction in transactions)
                        //{
                        //    // TODO: Implement logic to write transactions to output file in standardized format.
                        //    Console.WriteLine(transaction);
                        //}
                    }
                    else
                    {
                        AnsiConsole.MarkupLine($"[yellow]Warning:[/] No transactions found in file [blue]{file}[/]");
                    }
                }
                catch (Exception ex)
                {
                    AnsiConsole.MarkupLine($"[red]Error:[/] Failed to process file [blue]{file}[/]. Exception: {ex.Message}");
                    throw;
                }
            }
        }
    };

}
