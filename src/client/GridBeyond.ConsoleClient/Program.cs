using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Core;

namespace GridBeyond.ConsoleClient
{
    class Program
    {
        static Logger log;

        static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            log = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            var optionChoosen = 0;
            while(optionChoosen != 9)
            {
                do
                {
                    ShowOptions();
                    var input = Console.ReadLine();
                    int.TryParse(input, out optionChoosen);
                } while (!new[] { 1, 2, 3 }.Contains(optionChoosen));

                switch (optionChoosen)
                {
                    case 1:
                        InsertRecords();
                        break;
                    case 2:
                        ShowReport();
                        break;
                    case 9:
                        Environment.Exit(0);
                        break;
                }
            }
        }

        private static void ShowOptions()
        {
            log.Information("Please, choose one of the options below:");
            log.Information("1. Insert Records");
            log.Information("2. Get Report.");
            log.Information("9. Quit.");
        }

        private static void InsertRecords()
        {
            log.Information("Please, inform the path to the \'.csv\' file.");
            var path = Console.ReadLine();

            log.Information("Reading from file...");
            var csvRecords = Task.Run(() => ReadFile(path)).GetAwaiter().GetResult();
            log.Information($"{csvRecords.Count()} recored read.");

            log.Information("Uploading to server...");
            var result = Task.Run(() => HttpHelper.SendRecords(csvRecords)).GetAwaiter().GetResult();

            log.Information($"{result.ValidRecords.Count} valid records.");
            log.Information($"{result.InvalidRecords.Count} malformed records.");
            if (result.InvalidRecords.Any())
            {
                log.Warning("INVALID RECORD IN LINES:");
                foreach (var record in result.InvalidRecords)
                    log.Warning(record.ToString());
            }
            log.Information($"{result.NewRecords.Count} new records.");
        }

        private static void ShowReport()
        {
            var report = Task.Run(() => HttpHelper.GetReportData()).GetAwaiter().GetResult();

            log.Information("============REPORT============");
            log.Information($"Total records: {report.TotalRecords}");
            log.Information($"Average Value: {report.AverageValue}");
            log.Information($"Highest Value: {report.HighestValue} on {report.HighestValueDate}");
            log.Information($"Lowest Value: {report.LowestValue} on {report.LowestValueDate}");
            log.Information("============REPORT============");
        }

        private static async Task<string[]> ReadFile(string path)
        {
            while (!File.Exists(path))
            {
                log.Information("Could not find the specified file.");
                log.Information("Please, inform the path to the \'.csv\' file.");
                path = Console.ReadLine();
            }
            return await File.ReadAllLinesAsync(path);
        }
    }
}
