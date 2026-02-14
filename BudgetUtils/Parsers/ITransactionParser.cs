using BudgetUtils.Models;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

/* 
 * This interface defines the contract for parsing transaction data from various sources (e.g., CSV files, APIs).
 * Implementations of this interface will be responsible for reading transaction data and converting it into a standardized format that can be used by the rest of the application.
 * This allows for flexibility in supporting different data sources and formats without changing the core logic of the application.
 */

namespace BudgetUtils.Parsers
{
    public interface ITransactionParser
    {
        /// <summary>
        /// Provides the mapping configuration for the parser.
        /// </summary>
        /// <returns>A CsvHelper ClassMap for the specific parser.</returns>
        public ClassMap<BankTransaction> GetParserMap();

        /// <summary>
        /// Parses transaction data from a given source and returns a list of transactions.
        /// </summary>
        /// <param name="source">The source of the transaction data (e.g., file path, API endpoint).</param>
        /// <returns>A list of parsed transactions.</returns>
        public IEnumerable<BankTransaction> ParseTransactions(string source);
    }
}
