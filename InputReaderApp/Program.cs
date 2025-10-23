using InputReaderApp.Readers;
using InputReaderApp.Utils;
using InputReaderApp.Writers;

namespace InputReaderApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            var json = @"[
  { ""Name"": ""Alice"", ""Age"": 30, ""City"": ""London"" },
  { ""Name"": ""Bob"", ""Age"": 25, ""City"": ""New York"" },
  { ""Name"": ""Charlie"", ""Age"": 35, ""City"": ""Paris"" }
]";

            var writer = new JSONFileTableWriter();
            var result = writer.Write(json, "output.txt");

            if (result.IsSuccess)
                Console.WriteLine("JSON table written successfully!");
            else
                Console.WriteLine($"Failed: {result.Message}");
        }
    }
}
//<Employees>
//Galin 36 5000
//Georgi 24 2500
//Ivan 40 1000

//<Meetings>
//Sofia 2h Galin Ivan Georgi
//Varna 1h Ivan Galin


//<Locations>
//Sofia "Slaveykov 1"
//Burgas "Ivan Vazov 3"
//Varna "Baba tonka 50"