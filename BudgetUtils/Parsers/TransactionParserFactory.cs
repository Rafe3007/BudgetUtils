using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetUtils.Parsers
{
    public class TransactionParserFactory
    {
        public static ITransactionParser GetParser(string filepath)
        {
            string filename = System.IO.Path.GetFileName(filepath).ToLower();
            if (filename.Contains("chase"))
            {
                return new ChaseFreedomParser();
            }
            else if (filename.Contains("exportedtransaction"))
            {
                return new CanvasParser();
            }
            else
            {
                return new GenericParser();
            }
        }
    }
}
