using System;
using System.Globalization;
using System.IO;

namespace KvircLogExporter
{
    class KvircLogFile
    {
        public KvircLogFile(string filePath, string dtFormat = "yyyy.MM.dd")
        {
            FilePath = filePath;

            Date = DateTime.ParseExact(Path.GetFileNameWithoutExtension(FileName).Substring(FileName.LastIndexOf("_") + 1),
                dtFormat, CultureInfo.InvariantCulture);
        }

        public string FilePath { get; }
        public string FileName => Path.GetFileName(FilePath);
        public DateTime Date { get; }

        public override string ToString()
        {
            return $"{Date}, {nameof(FilePath)}: {FilePath}, {nameof(FileName)}: {FileName}";
        }
    }
}