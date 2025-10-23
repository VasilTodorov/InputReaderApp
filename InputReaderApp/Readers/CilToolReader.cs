using InputReaderApp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InputReaderApp.Readers
{
    public record Command(string Input, string Output, bool Quiet);
    public class CilToolReader : ReaderBase<Command>
    {
        public CilToolReader(TextReader? input = null) : base(input)
        {
        }

        public override Result<Command> Read()
        {
            string? line = Input.ReadLine();          
            if (string.IsNullOrWhiteSpace(line))
            {
                return Result<Command>.Fail(ErrorCode.InputNotFound, "Error(1) : input not found");
            }

            var tokens = line.Split(' ',StringSplitOptions.RemoveEmptyEntries);
            string? input = null ;
            string? output = null;
            bool quiet = false;            

            if (tokens[0] != "command")
                return Result<Command>.Fail(ErrorCode.InvalidFormat, "Error(2) : command not found");

            for ( int i = 1; i < tokens.Length; i++)
            {
                switch (tokens[i])
                {
                    case "-f":
                        if (input is not null)
                            return Result<Command>.Fail(ErrorCode.InvalidFormat, "Error(3) : Duplicate input option (-f) found");
                        if (i+1<tokens.Length) 
                            input = tokens[++i]; 
                        break;
                    case "-o":
                        if (output is not null)
                            return Result<Command>.Fail(ErrorCode.InvalidFormat, "Error(4) :Duplicate output option (-o) found");
                        if (i + 1 < tokens.Length) 
                            output = tokens[++i];
                        break;
                    case "--quiet":
                        quiet = true;
                        break;
                    default:
                        return Result<Command>.Fail(ErrorCode.InvalidFormat, $"Error(5) : couldn't recognize: {tokens[i]}.");
                }
            }
            if(input is null)
                return Result<Command>.Fail(ErrorCode.InvalidFormat, "Error(6) : input not found");
            if (output is null)
                return Result<Command>.Fail(ErrorCode.InvalidFormat, "Error(7) : output not found");

            return Result<Command>.Success(new Command(input, output, quiet));
        }
    }
}
