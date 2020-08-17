using System;
using System.IO;
using System.Threading.Tasks;

namespace GridBeyond.ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Please, inform the path to the \'.csv\' file.");
            var path = "/Users/juliao/Projects/Interviews/GridBeyond/src/client/GridBeyond.ConsoleClient/data/MarketData.csv";//Console.ReadLine();

            var csvRecords = Task.Run(() => ReadFile(path)).GetAwaiter().GetResult();

            var result = Task.Run(() => HttpHelper.SendRecords(csvRecords)).GetAwaiter().GetResult();
        }

        private static async Task<string[]> ReadFile(string path)
        {
            while (!File.Exists(path))
            {
                Console.WriteLine("Could not find the specified file.");
                Console.WriteLine("Please, inform the path to the \'.csv\' file.");
                path = Console.ReadLine();
            }
            return await File.ReadAllLinesAsync(path);
        }
    }
}
