using BudgetUtils.Config;
using BudgetUtils.Services;
using Microsoft.Extensions.Configuration;
using Spectre.Console;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

var appSettings = configuration.GetSection("BudgetUtils").Get<AppSettings>();
if (appSettings == null)
{
    Markup.FromInterpolated($"[red]appSettings is null, please ensure it is properly configured");
    return;
}

string inputPath = PathResolver.ResolveInputPath(appSettings.InputPath);

// TODO: make table display of steps and progress bar
AnsiConsole.MarkupLine($"[DarkSlateGray2]Fetching all transaction .csv files from[/]" +
    $"[yellow]{inputPath}[/] " +
    $"[DarkSlateGray2]for the last[/] " +
    $"[yellow]{appSettings.DateRange.TimeString()}" +
    $"[/][DarkSlateGray2]...[/]");

TransactionService service = new(inputPath, appSettings);
try
{
    service.RunService();
}
catch (Exception ex)
{
    AnsiConsole.MarkupLine($"[red]An error occurred while running the transaction service: {ex.Message}[/]");
}
