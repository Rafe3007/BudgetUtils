using BudgetUtils.Models;
using System;
using System.Collections.Generic;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;

namespace BudgetUtils.Parsers
{
    internal class ChaseFreedomParser : ITransactionParser
    {
        string Name => "Chase Freedom";
        string FilePattern => "chase*.csv";

        public ClassMap<BankTransaction> GetParserMap()
        {
            return new ChaseParserMap();
        }

        /* This class defines the mapping between the CSV columns and the BankTransaction properties for Chase Freedom transactions.
         * required by CsvHelper to correctly parse the CSV file.
         */
        public sealed class ChaseParserMap : ClassMap<BankTransaction> 
        {
            public ChaseParserMap()
            {
                Map(m => m.TransactionDate).Name("Transaction Date");
                Map(m => m.PaidTo).Name("Description");
                Map(m => m.Amount).Name("Amount");
            }
        }

        public IEnumerable<BankTransaction> ParseTransactions(string source)
        {
            try
            {
                using (var reader = new StringReader(source))
                using (var csv = new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture))
                {
                    csv.Context.RegisterClassMap(GetParserMap());
                    var records = csv.GetRecords<BankTransaction>();
                    return [.. records];
                }
            }
            catch (Exception ex)
            {
                throw new FormatException("Failed to parse Chase Freedom transactions: ", ex);
            }
        }
    }
}
