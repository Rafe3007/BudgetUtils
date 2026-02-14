using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace BudgetUtils.Utils
{
    public static class CsvConfigurationFactory
    {
        public static CsvConfiguration CreateDefault()
        {
            return new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                PrepareHeaderForMatch = args =>
                    Regex.Replace(args.Header, @"[^a-zA-Z0-9]", "")
                     .ToLowerInvariant(),
                MissingFieldFound = null,
                HeaderValidated = null,
                Delimiter = ",",
            };
        }
    }

}
