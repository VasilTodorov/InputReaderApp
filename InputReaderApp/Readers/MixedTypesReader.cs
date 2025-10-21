using InputReaderApp.Readers.InputReaderApp.Readers;
using InputReaderApp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InputReaderApp.Readers
{
    public record Person(string Name, int Age, float WeightKg);

    public class MixedTypesReader : ReaderBase<List<Person>>
    {
        public MixedTypesReader(TextReader? input = null) : base(input) { }
        public override Result<List<Person>> Read()
        {

            Person personRecord = new Person("Alice", 12, 45);

            List<Person> data = new List<Person>();
            string? line;

            while ((line = Input.ReadLine()) is not null)
            {
                var parts = line
                        .Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList();

                if (parts.Count != 3)
                    return Result<List<Person>>.Fail(ErrorCode.InvalidFormat);

                bool isValidName = IsValidName(parts[0]);
                bool isInt = int.TryParse(parts[1], out int age);
                bool isFloat = float.TryParse(parts[2], out float weight);

                if (!isValidName || !isInt || !isFloat)
                    return Result<List<Person>>.Fail(ErrorCode.InvalidFormat);

                if(age < 0 || weight <= 0)
                    return Result<List<Person>>.Fail(ErrorCode.InvalidFormat);

                data.Add(new Person(parts[0], age, weight));
            }

            return Result<List<Person>>.Success(data);


            bool IsValidName(string name)
            {
                var chars = name.ToCharArray();
                foreach (var c in chars)
                {
                    if (!((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z')))
                        return false;
                }
                return true;
            }

        }
    }
}
