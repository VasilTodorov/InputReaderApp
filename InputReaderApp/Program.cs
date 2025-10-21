using InputReaderApp.Readers;

namespace InputReaderApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            var reader = new JaggedArrayReader();
            var result = reader.Read();
            Console.WriteLine(result.IsSuccess ? "List" : "Invalid");
            //Console.WriteLine("Hello, World!");
        }
    }
}
