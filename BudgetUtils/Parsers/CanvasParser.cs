using BudgetUtils.Models;
using BudgetUtils.Utils;
using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.RegularExpressions;

namespace BudgetUtils.Parsers
{
    internal class CanvasParser : ITransactionParser
    {
        const string BankName = "Canvas Credit";

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
                Map(m => m.TransactionDate).Name("postingdate");
                Map(m => m.BankName).Constant(BankName);
                Map(m => m.PaidTo).Name("description");
                Map(m => m.Amount).Name("amount");
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
                    var records = csv.GetRecords<BankTransaction>().ToList();
                    return records;
                }
            }
            catch (Exception ex)
            {
                throw new FormatException("Failed to parse Chase Freedom transactions: ", ex);
            }
        }
    }
}
