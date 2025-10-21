using InputReaderApp.Readers.InputReaderApp.Readers;
using InputReaderApp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace InputReaderApp.Readers
{
    public class NestedListReader : ReaderBase<List<List<int>>>
    {
        public NestedListReader(TextReader? input = null) : base(input) { }
        
        public override Result<List<List<int>>> Read()
        {
            try
            {
                List<List<int>> data = new List<List<int>>();
                string? line;
                
                while ((line = Input.ReadLine()) is not null)
                {
                    var row = line
                            .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                            .Select(x => int.Parse(x))
                            .ToList();

                    data.Add(row);                    
                }

                return Result<List<List<int>>>.Success(data);
            }
            catch
            {
                return Result<List<List<int>>>.Fail(ErrorCode.InvalidFormat);
            }

        }
    }
}
