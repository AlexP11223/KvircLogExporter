using System;
using System.IO;
using System.Linq;

namespace KvircLogExporter
{
    class Program
    {
        private static readonly string LogDir = @"C:\Users\Alex\Dropbox\Backup\gsoc\logs\";

        private static string GenerateExportScript(string name)
        {
            var files = Directory.GetFiles(LogDir + @"raw\")
                .Where(f => f.ToLower().Contains(name.ToLower()));

            var result = string.Join(Environment.NewLine,
                files.Select(f => $"echo $log.export({f});"));

            return result;
        }

        private static void ConcatFiles(string name, string outputFilePath)
        {
            var files = Directory.GetFiles(LogDir + $@"txt\{name}\")
                .Select(f => new KvircLogFile(f))
                .OrderBy(f => f.Date);

            var texts = files.Select(logFile =>
            {
                var lines = File.ReadAllLines(logFile.FilePath);
                var filteredLines = lines.Where(l => l.StartsWith("###") || l.Contains("] <")).ToList();
                if (!filteredLines.Any(l => l.Contains("] <")))
                {
                    return "";
                }
                return string.Join("\r\n", filteredLines);
            }).Where(s => !string.IsNullOrWhiteSpace(s));

            string result = string.Join("\r\n\r\n", texts).Trim();

            File.WriteAllText(LogDir + $@"{name}.txt", result);
        }

        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Specify dir path");
                return;
            }

            string inputPath = args[0];

            string filter = args.Length >= 2 ? args[1] : null;
            string outputFilePath = Directory.GetParent(inputPath).FullName + $"/" + (filter ?? "log") + ".txt";
            if (args.Length >= 3)
            {
                outputFilePath = args[2];
            }

            Console.WriteLine($"{inputPath} *{filter}* -> {outputFilePath}");

            new LogExporter(inputPath, filter).Concat(outputFilePath);
        }
    }
}
