using InputReaderApp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InputReaderApp.Readers
{
    public class NestedListReader : ReaderBase<List<List<int>>>
    {
        public NestedListReader(TextReader? input = null) : base(input) { }        
        public override Result<List<List<int>>> Read()
        {
            List<List<int>> result = new List<List<int>>();
            string ? line = Input.ReadLine();

            if (line is null) 
                return Result<List<List<int>>>.Fail(ErrorCode.InputNotFound, "Error: No input");

            line = line.Trim();
            if (!(line.StartsWith('[') && line.EndsWith(']')))
                return Result<List<List<int>>>.Fail(ErrorCode.InvalidFormat, "Error: Main List must be between []");

            line = line.Substring(1,line.Length-2).Trim();

            if(line=="")
                return Result<List<List<int>>>.Success(result);

            //List<string> parts = line.Split(',').Select(p=>p.Trim()).ToList();
            var resultParts = ListSpliter(line);

            if (resultParts.IsFailure)
                return Result<List<List<int>>>.Fail(ErrorCode.InvalidFormat, resultParts.Message);

            List<string> parts = resultParts.Data!;
            foreach (string part in parts)
            {
                if (!(part.StartsWith('[') && part.EndsWith(']')))
                    return Result<List<List<int>>>.Fail(ErrorCode.InvalidFormat, "Error: Inner List must be between []");

                string innerPart = part.Substring(1, part.Length - 2).Trim();
                if(innerPart=="")
                    result.Add(new List<int>());
                else
                    try
                    {
                        List<int> innerList = innerPart
                                            .Split(",")
                                            .Select(p => int.Parse(p.Trim()))
                                            .ToList();
                        result.Add(innerList);
                    }
                    catch
                    {
                        return Result<List<List<int>>>.Fail(ErrorCode.InvalidFormat, "Error: Inner List for");
                    }
                    
            }
            return Result<List<List<int>>>.Success(result);
        }
        private Result<List<string>> ListSpliter(string line)
        {                        
            List<string> lists = new List<string>();
            line = line.Trim();
            while (line != string.Empty)
            {
                if (!line.StartsWith("["))
                    return Result<List<string>>.Fail(ErrorCode.InvalidFormat, "Inner list doesn't star with '['");
                int listEndIndex = line.IndexOf("]");
                if (listEndIndex == -1)
                    return Result<List<string>>.Fail(ErrorCode.InvalidFormat, "Inner list doesn't end with ']'");
                else
                {
                    string listString = line.Substring(0, listEndIndex + 1);
                    lists.Add(listString);
                    line = line.Substring(listEndIndex + 1).Trim();
                }
                if (line.StartsWith(","))
                {
                    line = line.Substring(1).Trim();
                    if(line == string.Empty) return Result<List<string>>.Fail(ErrorCode.InvalidFormat, "Unnecessary ',' at the end");
                }                   
            }

            return Result<List<string>>.Success(lists);
        }
    }
}
