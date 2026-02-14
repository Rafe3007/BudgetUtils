using BudgetUtils.Models;
using BudgetUtils.Utils;
using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetUtils.Parsers
{
    public class ChaseFreedomParser : ITransactionParser
    {
        const string BankName = "Chase Freedom";

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
                Map(m => m.TransactionDate).Name("transactiondate");
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
