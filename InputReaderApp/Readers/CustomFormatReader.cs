using InputReaderApp.Readers;
using InputReaderApp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InputReaderApp.Readers
{
    public record PersonGame(string Name, int Age, int Score);
    public class CustomFormatReader : ReaderBase<PersonGame>
    {
        public CustomFormatReader(TextReader? input = null) : base(input) { }
        public override Result<PersonGame> Read()
        {
            string? line = Input.ReadLine();
            if (line is null)
                return Result<PersonGame>.Fail(ErrorCode.InputNotFound);

            var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 3)
                return Result<PersonGame>.Fail(ErrorCode.InvalidFormat);

            var keyValuePaires = parts.Select(x => x.Split(":")).ToArray();
            
            Dictionary<string, string> dict = new Dictionary<string, string>();

            foreach (var keyValue in keyValuePaires)
            {
                if (keyValue.Length != 2) return Result<PersonGame>.Fail(ErrorCode.InvalidFormat);
                switch (keyValue[0])
                {
                    case "name":
                        dict["name"] = keyValue[1];
                        break;
                    case "age":
                        dict["age"] = keyValue[1];
                        break;
                    case "score":
                        dict["score"] = keyValue[1];
                        break;
                }

            }
            if(!dict.ContainsKey("name") || !dict.ContainsKey("age") || !dict.ContainsKey("score"))
                return Result<PersonGame>.Fail(ErrorCode.InvalidFormat);

            if(!int.TryParse(dict["age"], out int age) || !int.TryParse(dict["score"], out int score) || !Helpers.IsValidName(dict["name"]))
                return Result<PersonGame>.Fail(ErrorCode.InvalidFormat);

            return Result<PersonGame>.Success(new PersonGame(dict["name"], age, score));
        }
    }
}
