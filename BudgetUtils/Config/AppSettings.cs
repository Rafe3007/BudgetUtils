using System;
using System.Collections.Generic;
using System.Text;

/* 
 * Making a class is necessary to support strongly typed configuration. 
 * This class will be used to bind the configuration values from appsettings.json or other configuration sources.
 */ 

namespace BudgetUtils.Config
{
    class AppSettings
    {
        public required DateRange DateRange { get; set; }
        public required string OutputPath { get; set; }
        public required string InputPath { get; set; }
    }

    class DateRange
    {
        public required int Interval { get; set; }
        public required string Unit { get; set; }
        public string TimeString() {
            int interval = Interval;
            string u = Unit.ToLower();
            return u switch
            {
                "seconds" => $"{interval} Seconds(s)",
                "minutes" => $"{interval} Minutes(s)",
                "hours" => $"{interval} Hours(s)",
                _ => $"BAD UNIT: {u}"
            };
        }
    }
}
