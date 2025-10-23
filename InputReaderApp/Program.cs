using InputReaderApp.Readers;
using InputReaderApp.Utils;

namespace InputReaderApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            var reader = new ExpressionReader(new StringReader("3 + 5 * (2( - 1))"));
            //double a = 2 2;

            //string s = "2 2"; // copy-paste your actual string here
            //foreach (char c in s)
            //    Console.WriteLine($"'{c}' => {(int)c}");

            //if (float.TryParse(s, out float number))
            //    Console.WriteLine(number);
            //else
            //    Console.WriteLine("\"2 2\" is not a number");

            // Act
            Result<double> result = reader.Read();
            if (result.IsFailure) {
                Console.WriteLine("Failer");
            }else
            {
                Console.WriteLine($"Result: {result.Data}");
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