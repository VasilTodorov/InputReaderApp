using InputReaderApp.Apps;
using InputReaderApp.Readers;
using InputReaderApp.Utils;
using InputReaderApp.Writers;

namespace InputReaderApp
{
    public class Program
    {
        static void Main(string[] args)
        {           
            DynamicFileWorker worker = new DynamicFileWorker();
            var result =worker.Work("D:\\workspace\\C#OOP\\Training\\Practice\\InputReaderApp\\InputReaderApp\\Workers\\Input",
                                    "D:\\workspace\\C#OOP\\Training\\Practice\\InputReaderApp\\InputReaderApp\\Workers\\Output");
            if (result.IsFailure)
            {
                Console.WriteLine("Something didn't work");
            }
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