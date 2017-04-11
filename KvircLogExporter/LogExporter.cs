using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace KvircLogExporter
{
    public class LogExporter
    {
        public string Newline = "\r\n";

        private readonly List<KvircLogFile> _inputFiles;

        public LogExporter(string inputPath, string filter = null, string dtFormat = "yyyy.MM.dd")
        {
            _inputFiles = Directory.GetFiles(inputPath)
                .Where(f => string.IsNullOrEmpty(filter) || f.ToLower().Contains(filter.ToLower()))
                .Select(f => new KvircLogFile(f, dtFormat))
                .OrderBy(f => f.Date)
                .ToList();
        }

        public void Concat(string outputFilePath)
        {
            var texts = _inputFiles.Select(logFile =>
            {
                var lines = File.ReadAllLines(logFile.FilePath);
                var filteredLines = lines.Where(l => l.StartsWith("###") || l.Contains("] <")).ToList();
                if (!filteredLines.Any(l => l.Contains("] <")))
                {
                    return "";
                }
                return string.Join(Newline, filteredLines);
            }).Where(s => !string.IsNullOrWhiteSpace(s));

            string result = string.Join(Newline + Newline, texts).Trim();

            File.WriteAllText(outputFilePath, result);
        }
    }
}
