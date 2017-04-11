using System;
using System.IO;

namespace KvircLogExporter
{
    class Program
    {
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
