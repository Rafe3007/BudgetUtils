using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetUtils.Services
{
    internal class TransactionService
    {
        // Fetches transactions from the data source and processes them as needed.
    }

    List<string> GetTransactionSources(string inputDirectory) {
        try
        {
            string[] files = Directory.GetFiles(inputDirectory, "*.csv");
            return new List<string>(files);
        } catch (Exception ex) {
            throw new IOException($"Failed to fetch transaction sources from {inputDirectory}: ", ex);
        }
    }
}
