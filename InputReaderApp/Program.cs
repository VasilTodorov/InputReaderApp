using InputReaderApp.Readers;

namespace InputReaderApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            DifferentStatesReader reader = new DifferentStatesReader();

            Console.WriteLine(reader.Read().IsSuccess ? "Success" : "Failer");
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