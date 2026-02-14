using System;
using System.Collections.Generic;
using System.Text;

/* 
 * Making a class is necessary to support strongly typed configuration. 
 * This class will be used to bind the configuration values from appsettings.json or other configuration sources.
 */ 

namespace BudgetUtils.Config
{
    public class AppSettings
    {
        public required DateRange DateRange { get; set; }
        public required string OutputPath { get; set; }
        public required string InputPath { get; set; }
    }

    public class DateRange
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
                "days" => $"{interval} Days(s)",
                "weeks" => $"{interval} Weeks(s)",
                "months" => $"{interval} Months(s)",
                "years" => $"{interval} Years(s)",

                _ => $"BAD UNIT: {u}"
            };
        }

        public DateTime GetDate() { 
            int interval = Interval;
            string u = Unit.ToLower();
            return u switch
            {
                "seconds" => DateTime.Now.AddSeconds(-interval),
                "minutes" => DateTime.Now.AddMinutes(-interval),
                "hours" => DateTime.Now.AddHours(-interval),
                "days" => DateTime.Now.AddDays(-interval),
                "weeks" => DateTime.Now.AddDays(-interval * 7),
                "months" => DateTime.Now.AddMonths(-interval),
                "years" => DateTime.Now.AddYears(-interval),

                _ => throw new FormatException($"Invalid time unit: {u}")
            };
        }
    }
}
