using BudgetUtils.Models;
using BudgetUtils.Utils;
using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace BudgetUtils.Parsers
{
    internal class GenericParser : ITransactionParser
    {
        public ClassMap<BankTransaction> GetParserMap()
        {
            return new GenericParserMap();
        }

        /* This class defines the mapping between the CSV columns and the BankTransaction properties for transactions.
         * required by CsvHelper to correctly parse the CSV file.
         */
        public sealed class GenericParserMap : ClassMap<BankTransaction>
        {
            public GenericParserMap()
            {
                Map(m => m.TransactionDate)
                    .Name("postingdate", "transactiondate", "postdate");
                Map(m => m.BankName).Constant("Unkown Bank");
                Map(m => m.PaidTo)
                    .Name("description", "merchant", "payee", "memo");
                Map(m => m.Amount)
                    .Name("amount", "transactionamount", "debit", "credit");
            }
        }

        public IEnumerable<BankTransaction> ParseTransactions(string filepath)
        {
            try
            {
                using (var reader = new StreamReader(filepath))
                using (var csv = new CsvReader(reader, CsvConfigurationFactory.CreateDefault()))
                {
                    csv.Context.RegisterClassMap(GetParserMap());
                    var records = csv.GetRecords<BankTransaction>();
                    return [.. records];
                }
            }
            catch (Exception ex)
            {
                throw new FormatException($"Failed to parse transactions for {PathResolver.GetFileNameFromPath(filepath)}", ex);
            }
        }
    }
}
