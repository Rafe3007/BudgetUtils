using BudgetUtils.Models;
using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetUtils.Parsers
{
    internal class CanvasParser : ITransactionParser
    {
        string Name => "Canvas";
        string FilePattern => "ExportedTransaction*.csv";

        public ClassMap<BankTransaction> GetParserMap()
        {
            return new CanvasParserMap();
        }

        /* This class defines the mapping between the CSV columns and the BankTransaction properties for Chase Freedom transactions.
         * required by CsvHelper to correctly parse the CSV file.
         */
        public sealed class CanvasParserMap : ClassMap<BankTransaction>
        {
            public CanvasParserMap()
            {
                Map(m => m.TransactionDate).Name("Posting Date");
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
