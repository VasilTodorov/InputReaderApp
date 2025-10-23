using InputReaderApp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InputReaderApp.Writers
{
    public class FileWriter : IOutputWriter<string>
    {
        public Result Write(string value, string outputFilePath)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(outputFilePath))
                    return Result.Fail(ErrorCode.InputNotFound,"Output file path is null or empty.");

                // Ensure the directory exists
                var directory = Path.GetDirectoryName(outputFilePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                    Directory.CreateDirectory(directory);

                // Write to file
                File.WriteAllText(outputFilePath, value);                

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Fail(ErrorCode.InternalError,$"Error writing to file: {ex.Message}");
            }
        }
    }
}
