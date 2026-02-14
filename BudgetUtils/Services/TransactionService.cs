using BudgetUtils.Models;
using BudgetUtils.Parsers;
using Spectre.Console;
using BudgetUtils.Config;
using BudgetUtils.Utils;

namespace BudgetUtils.Services
{
    internal class TransactionService(string inputDirectory, AppSettings configuration)
    {
        readonly string _inputDirectory = inputDirectory;
        readonly AppSettings _appSettings = configuration;

        // Fetches transactions from the data source and processes them as needed.
        public void RunService()
        {
            // Create utility inline - no service registration needed
            var fileProcessor = new RecursiveFileProcessor(_appSettings);
            List<string> files = fileProcessor.ProcessDirectory(_inputDirectory);
            string outputFilePath = Path.Combine(_inputDirectory, "output.csv");

            List<BankTransaction> allTransactions = new List<BankTransaction>();
            foreach (string file in files)
            {
                var parser = TransactionParserFactory.GetParser(file);
                try
                {
                    var transactions = parser.ParseTransactions(file);
                    if (transactions != null && transactions.Any())
                    {
                        foreach (var transaction in transactions)
                        {
                            allTransactions.Add(transaction);
                        }
                    }
                    else
                    {
                        AnsiConsole.MarkupLine($"[yellow]Warning:[/] No transactions found in file [blue]{file}[/]");
                    }
                }
                catch (Exception ex)
                {
                    AnsiConsole.MarkupLine($"[red]Error:[/] Failed to process file [blue]{file}[/]. Exception: {ex.Message}");
                    if (ex is not FormatException)
                        throw;
                }
            }

            using (var writer = new StreamWriter(outputFilePath))
            {
                writer.WriteLine("Date,Method,PaidTo,Amount");
                foreach (var transaction in allTransactions)
                {
                    string amountString = transaction.Amount.HasValue ? $"{Math.Round(transaction.Amount.Value, 2)}" : "Parse Error";
                    string line = $"{transaction.TransactionDate?.ToString("MM/dd/yyyy")},{transaction.BankName},{transaction.PaidTo},{amountString}";
                    writer.WriteLine(line);
                }
            }

        }
    };

}
