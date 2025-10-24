using InputReaderApp.Utils;
using InputReaderApp.Writers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Timers;

namespace InputReaderApp.Apps
{
    public class DynamicFileWorker
    {
        private System.Timers.Timer? _timer;
        private bool _isWorking = false;
        public Result Work(string inputDirectoryPath, string outputDirectoryPath)
        {
            try
            {
                string? inputDirectory = inputDirectoryPath;

                if (!string.IsNullOrEmpty(inputDirectory) && !Directory.Exists(inputDirectory))
                {
                    Directory.CreateDirectory(inputDirectory);
                }

                string? outputDirectory = outputDirectoryPath;

                if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                List<string> readFilePaths = new List<string>();

                _timer = new System.Timers.Timer(10000);
                _timer.Elapsed += (s, e) =>
                {
                    Console.WriteLine($"Tick at {DateTime.Now}");
                    ReadFiles(inputDirectoryPath, outputDirectoryPath, readFilePaths);
                };
                _timer.Start();

                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Fail(ErrorCode.InternalError, ex.Message);
            }

        }

        private void ReadFiles(string inputDirectory, string outputDirectory, List<string> readFilePaths)
        {
            if (_isWorking) return; // skip if still working
            _isWorking = true;

            try
            {
                var currentFilePaths = Directory.GetFiles(inputDirectory);
                List<string> newFilePaths = currentFilePaths.Where(p => !readFilePaths.Contains(p)).ToList();

                foreach (var filePath in newFilePaths)
                {
                    try
                    {
                        string content = SafeReadFile(filePath);
                        string destPath = Path.Combine(outputDirectory, Path.GetFileName(filePath));
                        File.WriteAllText(destPath, content);                        
                        Console.WriteLine($"Copied {filePath} -> {destPath}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error copying {filePath}: {ex.Message}");
                    }                    
                }

                readFilePaths.AddRange(newFilePaths);
            }
            finally
            {
                _isWorking = false;
            }

        }

        private string SafeReadFile(string path)
        {
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    return File.ReadAllText(path);
                }
                catch (IOException)
                {
                    Thread.Sleep(500);
                }
            }
            throw new IOException($"Unable to read {path} after 3 attempts.");
        }
    }
}
