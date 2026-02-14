using BudgetUtils.Models;
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
                Map(m => m.PaidTo)
                    .Name("description", "merchant", "payee", "memo");
                Map(m => m.Amount)
                    .Name("amount", "transactionamount", "debit", "credit");
            }
        }

        public IEnumerable<BankTransaction> ParseTransactions(string source)
        {
            try
            {
                using (var reader = new StringReader(source))
                using (var csv = new CsvReader(reader, CreateConfig()))
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

        private static CsvConfiguration CreateConfig()
        {
            return new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                PrepareHeaderForMatch = args =>
                    Regex.Replace(args.Header, @"[^a-zA-Z0-9]", "")
                         .ToLowerInvariant(),
                MissingFieldFound = null,
                HeaderValidated = null
            };
        }
    }
}
