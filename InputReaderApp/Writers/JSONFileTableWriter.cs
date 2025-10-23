using InputReaderApp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace InputReaderApp.Writers
{
    public class JSONFileTableWriter : IOutputWriter<string>
    {
        public Result Write(string outputFilePath,string json)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(outputFilePath))
                    return Result.Fail(ErrorCode.InputNotFound,"Output file path is null or empty.");

                // Deserialize JSON to a list of dictionaries
                var data = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(json);
                if (data == null || data.Count == 0)
                    return Result.Fail(ErrorCode.InvalidFormat,"JSON is empty or invalid.");

                // Convert to table string
                string table = ConvertToTable(data);

                // Ensure directory exists
                var directory = Path.GetDirectoryName(outputFilePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                    Directory.CreateDirectory(directory);

                // Write to file
                File.WriteAllText(outputFilePath, table);

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Fail(ErrorCode.InternalError,$"Error writing table to file: {ex.Message}");
            }
        }

        private string ConvertToTable(List<Dictionary<string, object>> data)
        {
            var headers = data[0].Keys.ToList();

            // Header row
            string table = string.Join("\t|", headers) + Environment.NewLine;

            // Separator
            table += string.Join("\t|", headers.Select(h => new string('-', Math.Max(h.Length, 3)))) + Environment.NewLine;

            // Data rows
            foreach (var row in data)
            {
                table += string.Join("\t|", headers.Select(h => row[h]?.ToString() ?? "")) + Environment.NewLine;
            }

            return table;
        }
    }
}
